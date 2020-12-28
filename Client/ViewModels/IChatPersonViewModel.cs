using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorChat.ViewModels
{
    public interface IChatPersonViewModel
    {
        public List<MessageDto> Messages { get; set; }
        public string ChatId { get; set; }
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string Text { get; set; }
        public bool LoadingGet { get; set; }
        public bool LoadingAddMessage { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public Task SendMessage();
        public Task GetMessages();
    }
}