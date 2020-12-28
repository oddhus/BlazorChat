using System;

namespace BlazorChat.Shared.Dtos
{
    public class MessageDto
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Sent { get; set; }
        public bool IsNotice => Text.StartsWith("[Notice]");

        public MessageDto(string sender, string text)
        {
            Sender = sender;
            Text = text;
        }
    }
}