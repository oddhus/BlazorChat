using System.Collections.Generic;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.Shared.Dtos
{
    public class ChatDto
    {
        public string ParticipantA { get; set; }
        public string ParticipantB { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}