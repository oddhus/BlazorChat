using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChat.Server
{
    public class ChatPerson : Hub
    {

        public async Task Broadcast(string userId, string message)
        {
            await Clients.Caller.SendAsync("Broadcast", userId, message);
            await Clients.User(userId).SendAsync("Broadcast", userId, message);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}