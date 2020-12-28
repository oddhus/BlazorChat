using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorChat.ViewModels
{
    public class ContactsViewModel : IContactsViewModel
    {
        //properties
        public string UserId { get; set; }
        public string AddFirstname { get; set; }
        public string AddLastname { get; set; }
        public string AddAddress { get; set; }
        public string SearchFirstname { get; set; }
        public string SearchLastname { get; set; }
        public bool LoadingGet { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public List<ContactDto> SearchResult { get; set; }
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

        public async Task FindUsers()
        {
            var response = await _httpClient.GetFromJsonAsync<ContactDto[]>("users/" +
                "?firstname=" + this.SearchFirstname + "&lastname=" + this.SearchLastname);
            LoadSearchResult(response);
        }

        public async Task GetContacts()
        {
            var response = await _httpClient.GetFromJsonAsync<ContactDto[]>("users/" + this.UserId + "/contacts");
            LoadCurrentObject(response);
        }

        public async Task AddContact(string id)
        {
            LoadingUpdate = true;
            try
            {
                ContactDto contactDto = this.SearchResult.Find(contact => contact.Id == id);
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
                await _httpClient.DeleteAsync("users/" + this.UserId + "/contacts/" + contactId);
                this.Contacts.RemoveAll(c => c.Id == contactId);
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

        public async Task StartChat(string contactId)
        {
            LoadingUpdate = true;
            try
            {
                var res = await _httpClient.PostAsJsonAsync<ContactDto>("chats/" + this.UserId + "/start/" + contactId, null);
                var contact = this.Contacts.Find(c => c.Id == contactId);
                contact = await res.Content.ReadFromJsonAsync<ContactDto>();
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

        private void LoadSearchResult(ContactDto[] users)
        {
            this.SearchResult = new List<ContactDto>();
            foreach (ContactDto user in users)
            {
                this.SearchResult.Add(user);
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