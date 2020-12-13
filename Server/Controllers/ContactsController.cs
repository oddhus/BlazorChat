using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Models;
using BlazorChat.Server.Models;
using MongoDB.Driver;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {

        private IMongoCollection<Contact> _contacts;

        public ContactsController(IMongoClient client)
        {
            var database = client.GetDatabase("BlazorChat");
            _contacts = database.GetCollection<Contact>("contacts");

        }

        [HttpGet]
        public List<ContactReadDto> Get()
        {
            IEnumerable<Contact> contacts = _contacts.Find(c => true).ToList();
            List<ContactReadDto> contactList = new List<ContactReadDto>();
            foreach (var contact in contacts)
            {
                contactList.Add(new ContactReadDto(contact.Id.ToString(), contact.Firstname, contact.Lastname, contact.Address));
            }
            return contactList;
        }
    }
}
