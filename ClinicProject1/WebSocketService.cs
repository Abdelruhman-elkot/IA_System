using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using Fleck;
using Microsoft.EntityFrameworkCore;
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
                string senderId = null;
                string targetId = null;
                string chatMessage = null;
                string messType = null;

                WSconnection.OnOpen = () =>
                {
                    Console.WriteLine("New connection!");
                };

                WSconnection.OnClose = async () =>
                {
                    Console.WriteLine("Connection closed.");
                    if (senderId != null && allSockets.TryRemove(senderId, out _))
                    {
                        // Broadcast offline status
                        foreach (var socket in allSockets.Values)
                        {
                            await socket.Send($"[System] offline:{senderId}");
                        }
                    }
                };

                WSconnection.OnMessage = async (message) =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var _context = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();

                    Console.WriteLine("Received: " + message);
                    var parts = message.Split(":", 4);
                    if (parts.Length < 4)
                    {
                        await WSconnection.Send("Invalid format. Expected: messType:senderId:targetId:message");
                        return;
                    }

                    messType = parts[0];
                    senderId = parts[1].ToLower().Trim();
                    targetId = parts[2].ToLower().Trim();
                    chatMessage = parts[3];

                    if (messType == "firstMess")
                    {
                        Console.WriteLine($"[WebSocketService] firstMess from: {senderId}, to: {targetId}");
                        allSockets[senderId] = WSconnection;

                        Console.WriteLine("[WebSocketService] Connected users:");
                        foreach (var socket in allSockets)
                        {
                            Console.WriteLine($"User ID: {socket.Key}, Address: {socket.Value.ConnectionInfo.ClientIpAddress}:{socket.Value.ConnectionInfo.ClientPort}");
                        }

                        await WSconnection.Send($"[System] You are connected as '{senderId}'");

                        // Broadcast to others that this user is now online
                        foreach (var socket in allSockets)
                        {
                            if (socket.Key != senderId)
                            {
                                await socket.Value.Send($"[System] online:{senderId}");
                            }
                        }

                        // Send chat history
                        var chatHistory = await _context.chatsHistories.Where(
                            c => (c.targetUserId == senderId && c.senderUserId == targetId) ||
                                 (c.targetUserId == targetId && c.senderUserId == senderId))
                            .OrderBy(c => c.id)
                            .ToListAsync();

                        foreach (var chat in chatHistory)
                        {
                            await WSconnection.Send($"[{chat.senderUserId}] to [{chat.targetUserId}]: {chat.chatMessage}");
                        }
                        return;
                    }

                    else if (messType == "sendMess")
                    {
                        Console.WriteLine($"[WebSocketService] sendMess from: {senderId}, to: {targetId}, message: {chatMessage}");

                        var messToBeSaved = new chatHistory
                        {
                            targetUserId = targetId,
                            senderUserId = senderId,
                            chatMessage = chatMessage
                        };
                        _context.chatsHistories.Add(messToBeSaved);
                        await _context.SaveChangesAsync();

                        if (allSockets.TryGetValue(targetId, out var targetSocket))
                        {
                            await targetSocket.Send($"[{senderId}] to [{targetId}]: {chatMessage}");
                        }
                        else
                        {
                            await WSconnection.Send($"User '{targetId}' is not online. They will receive the message when they reconnect.");
                        }
                    }

                    else if (messType == "checkOnline")
                    {
                        // Added feature: allow user to check if the other is online
                        bool isOnline = allSockets.ContainsKey(targetId);
                        await WSconnection.Send($"[System] status:{targetId}:{(isOnline ? "online" : "offline")}");
                        return;
                    }
                };
            });

            Console.WriteLine("WebSocket server started on ws://localhost:8181");
        }
    }
}
