using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;
using Microsoft.AspNetCore.Components;

using MongoDB.Bson.IO;

namespace BlazorChat.ViewModels
{
    public class ContactsViewModel : IContactsViewModel
    {
        //properties
        public string UserId { get; set; }
        public string AddFirstname { get; set; }
        public string AddLastname { get; set; }
        public string AddAddress { get; set; }
        public bool LoadingGet { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public List<ContactDto> Contacts { get; set; }
        private HttpClient _httpClient;

        //methods
        public ContactsViewModel()
        {
        }
        public ContactsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task GetContacts()
        {
            var response = await _httpClient.GetFromJsonAsync<ContactDto[]>("users/" + this.UserId + "/contacts");
            LoadCurrentObject(response);
        }

        public async Task AddContact()
        {
            LoadingUpdate = true;
            try
            {
                ContactDto contactDto = this;
                var response = await _httpClient.PostAsJsonAsync<ContactDto>("users/" + this.UserId + "/contacts", contactDto);
                LoadCurrentObject(await response.Content.ReadFromJsonAsync<ContactDto[]>());
                Failed = false;
                LoadingUpdate = false;
            }
            catch (System.Exception)
            {
                Failed = true;
                ErrorMessage = "Failed to add contact";
                LoadingUpdate = false;
            }
        }

        public async Task DeleteContact(string contactId)
        {
            LoadingUpdate = true;
            try
            {
                var res = await _httpClient.DeleteAsync("users/" + this.UserId + "/contacts/" + contactId);
                Failed = false;
                LoadingUpdate = false;
            }
            catch (System.Exception)
            {
                Failed = true;
                ErrorMessage = "Failed to delete contact";
                LoadingUpdate = false;
            }
        }

        private void LoadCurrentObject(ContactDto[] contacts)
        {
            this.Contacts = new List<ContactDto>();
            foreach (ContactDto contact in contacts)
            {
                this.Contacts.Add(contact);
            }
        }

        public static implicit operator ContactDto(ContactsViewModel contactsViewModel)
        {
            return new ContactDto
            {
                Firstname = contactsViewModel.AddFirstname,
                Lastname = contactsViewModel.AddLastname,
                Address = contactsViewModel.AddAddress,
            };
        }
    }
}