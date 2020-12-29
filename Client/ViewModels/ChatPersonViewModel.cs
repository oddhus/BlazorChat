using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

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
                if (!string.IsNullOrEmpty(ChatId) && !string.IsNullOrEmpty(Text))
                {
                    MessageDto message = this;
                    await _httpClient.PostAsJsonAsync<MessageDto>("chats/" + this.ChatId + "/message", message);
                    Messages.Add(message);
                    Failed = false;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = $"Exception das: {e.Message}";
            }
        }

        public async Task GetMessages()
        {
            LoadingGet = true;
            try
            {
                if (!string.IsNullOrEmpty(ChatId))
                {
                    var response = await _httpClient.GetAsync("chats/" + ChatId);
                    if (response.IsSuccessStatusCode)
                    {
                        var chat = await response.Content.ReadFromJsonAsync<ChatDto>();
                        Messages = chat.Messages;
                        if (chat.ParticipantA == SenderId)
                        {
                            RecieverId = chat.ParticipantB;
                        }
                        else
                        {
                            RecieverId = chat.ParticipantA;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        ErrorMessage = "You do not have access to this chat";
                    }
                    else
                    {
                        ErrorMessage = "Could not retrieve the chat";
                    }
                }
                LoadingGet = false;
            }
            catch (Exception e)
            {
                ErrorMessage = $"Exception: {e.Message}";
                LoadingGet = false;
            }
        }

        public static implicit operator MessageDto(ChatPersonViewModel chatPersonViewModel)
        {
            return new MessageDto(chatPersonViewModel.SenderName, chatPersonViewModel.Text);
        }
    }
}