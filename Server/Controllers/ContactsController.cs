using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorChat.Shared;
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
        public IEnumerable<Contact> Get()
        {
            var con = _contacts.Find(c => true).ToList();
            return con;
        }
    }
}
