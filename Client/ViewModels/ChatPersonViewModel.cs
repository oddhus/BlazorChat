using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorChat.ViewModels
{
    public class ChatPersonViewModel : IChatPersonViewModel
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

        private HttpClient _httpClient;

        //methods
        public ChatPersonViewModel()
        {
        }
        public ChatPersonViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task SendMessage()
        {
            try
            {
                MessageDto message = this;
                await _httpClient.PostAsJsonAsync<MessageDto>("chats/" + this.ChatId + "/message", message);
                Failed = false;
            }
            catch (System.Exception)
            {
                Failed = true;
                ErrorMessage = "Failed to post message";
            }
        }

        public async Task GetMessages()
        {
            LoadingGet = true;
            try
            {
                var chat = await _httpClient.GetFromJsonAsync<ChatDto>("chats/" + this.ChatId);
                Messages = chat.Messages;
                if (chat.ParticipantA == SenderId)
                {
                    RecieverId = chat.ParticipantB;
                }
                else
                {
                    RecieverId = chat.ParticipantA;
                }
                LoadingGet = false;
            }
            catch (Exception e)
            {
                LoadingGet = false;
                ErrorMessage = $"Exception: {e.Message}";
            }
        }

        public static implicit operator MessageDto(ChatPersonViewModel chatPersonViewModel)
        {
            return new MessageDto(chatPersonViewModel.SenderName, chatPersonViewModel.Text);
        }
    }
}