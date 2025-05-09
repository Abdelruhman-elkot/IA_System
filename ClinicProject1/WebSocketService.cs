using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using Fleck;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;

namespace ClinicProject1
{
    public class WebSocketService
    {
        private readonly ConcurrentDictionary<string, IWebSocketConnection> allSockets = new();
        private readonly IServiceScopeFactory _scopeFactory;
        public WebSocketService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var server = new WebSocketServer("ws://0.0.0.0:8181");

            server.Start(WSconnection =>
            {
                string senderUsername = null;
                string targetUser = null;
                string chatMessage = null;
                string messType = null;




                WSconnection.OnOpen = () =>
                {
                    Console.WriteLine("New connection!");
                };

                WSconnection.OnClose = () =>
                {
                    Console.WriteLine("Connection closed.");
                    if (senderUsername != null)
                        allSockets.TryRemove(senderUsername, out _);
                };

                WSconnection.OnMessage = async (message) =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var _context = scope.ServiceProvider.GetRequiredService<ClinicDbContext>(); 
                    Console.WriteLine("Received: " + message);
                    var parts = message.Split(":", 4);
                    if (parts.Length < 4)
                    {
                        await WSconnection.Send("Invalid message format. Expected format: messType:senderUsername:targetUser:chatMessage");
                        return;
                    }
                    messType = parts[0];
                    senderUsername = parts[1].ToLower();
                    targetUser = parts[2].ToLower();
                    chatMessage = parts[3];
                    if (messType == "firstMess")
                    {
                        allSockets[senderUsername] = WSconnection;
                        await WSconnection.Send($"[System] You are connected as '{senderUsername}'");
                        var chatHistory = await _context.chatsHistories.Where(
                            c => (c.targetUser == senderUsername && c.senderUsername == targetUser) ||
                            (c.targetUser == targetUser && c.senderUsername == senderUsername)) 
                            .OrderBy(c => c.id)
                            .ToListAsync();
                        foreach (var chat in chatHistory)
                        {
                            await WSconnection.Send($"[{chat.senderUsername}] [{chat.targetUser}] {chat.chatMessage}");
                        }
                        return;
                    }
                    else if (messType == "sendMess")
                    {
                        var messToBeSaved = new chatHistory
                        {
                            targetUser = targetUser,
                            chatMessage = chatMessage,
                            senderUsername = senderUsername
                        };
                        _context.chatsHistories.Add(messToBeSaved);
                        await _context.SaveChangesAsync();

                        if (allSockets.TryGetValue(targetUser, out var checkedTargetUser)) 
                            await checkedTargetUser.Send($"[{senderUsername}] [{targetUser}] {chatMessage}"); 
                        else
                            await WSconnection.Send($" User '{targetUser}' is not online. he will recive the message as soon as he opens the chat");
                    }
                };
            });

            Console.WriteLine("WebSocket server started on ws://localhost:8181");
        }
    }
}
