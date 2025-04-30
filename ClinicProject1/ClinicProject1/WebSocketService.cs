using Fleck;
using System.Collections.Concurrent;

namespace ClinicProject1
{
    public class WebSocketService
    {
        private readonly ConcurrentDictionary<string, IWebSocketConnection> allSockets = new();

        public void Start()
        {
            var server = new WebSocketServer("ws://0.0.0.0:8181");

            server.Start(WSconnection =>
            {
                string? username = null;

                WSconnection.OnOpen = () =>
                {
                    Console.WriteLine("New connection!");
                };

                WSconnection.OnClose = () =>
                {
                    Console.WriteLine("Connection closed.");
                    if (username != null)
                        allSockets.TryRemove(username, out _);//i don't need the removed value so discard it
                };

                WSconnection.OnMessage = message =>
                {
                    Console.WriteLine("Received: " + message);
                    if (message.StartsWith("username:"))
                    {
                        username = message.Substring(9).Trim();
                        allSockets[username] = WSconnection;
                        WSconnection.Send($"[System] You are connected as '{username}'");//to test
                        return;
                    }
                    if (message.StartsWith(("sendTo:")))
                    {
                        var parts = message.Split(":", 3); //maximum 3 parts
                        var targetUser = parts[1];
                        var cahtMessage = parts[2];
                        if (allSockets.TryGetValue(targetUser, out var checkedTargetUser)) //et'kdna en fe 7ad bel esm da w sglnah fe checkedTargetUser
                            checkedTargetUser.Send($"[{username}] {cahtMessage}"); //"[Alice] Hello, Bob!"
                        else
                            WSconnection.Send($"[System] User '{targetUser}' is not connected.");

                    }
                };
            });

            Console.WriteLine("WebSocket server started on ws://localhost:8181");
        }
    }
}
