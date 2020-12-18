using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public interface IContactsViewModel
    {
        public List<ContactReadDto> Contacts { get; set; }
        public Task GetContacts();
    }
}