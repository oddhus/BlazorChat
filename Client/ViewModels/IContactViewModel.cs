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
        public List<ContactDto> Contacts { get; set; }
        public Task GetContacts();
        public Task AddContact();
        public Task DeleteContact(string contactId);
    }
}