using System.Linq;
using BlazorChat.Server.Models;
using Microsoft.AspNetCore.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        //return connection.User.Identity.Name;
    }
}