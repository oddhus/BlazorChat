using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorChat.ViewModels
{
    public interface IContactsViewModel
    {
        public string UserId { get; set; }
        public string AddFirstname { get; set; }
        public string AddLastname { get; set; }
        public string AddAddress { get; set; }
        public bool LoadingGet { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchFirstname { get; set; }
        public string SearchLastname { get; set; }
        public event Action OnChange;
        public List<ContactDto> Contacts { get; set; }
        public List<ContactDto> SearchResult { get; set; }
        public Task GetContacts();
        public Task AddContact(string id);
        public Task FindUsers();
        public Task StartChat(string contactId);
        public Task DeleteContact(string contactId);
    }
}