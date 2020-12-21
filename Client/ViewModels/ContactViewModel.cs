using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class ContactsViewModel : IContactsViewModel
    {
        //properties
        public List<ContactReadDto> Contacts { get; set; }
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
            var response = await _httpClient.GetFromJsonAsync<ContactReadDto[]>("users/contacts");
            LoadCurrentObject(response);
        }

        private void LoadCurrentObject(ContactReadDto[] contacts)
        {
            this.Contacts = new List<ContactReadDto>();
            foreach (ContactReadDto contact in contacts)
            {
                this.Contacts.Add(contact);
            }
        }


    }
}