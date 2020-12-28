using System;

namespace BlazorChat.Server.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Sent { get; set; }
    }
}