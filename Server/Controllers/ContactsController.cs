﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using BlazorChat.Server.Models;
using MongoDB.Driver;
using AutoMapper;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {

        private IMongoCollection<Contact> _contacts;
        private readonly IMapper _mapper;


        public ContactsController(IMongoClient client, IMapper mapper)
        {
            _mapper = mapper;
            var database = client.GetDatabase("BlazorChat");
            _contacts = database.GetCollection<Contact>("contacts");
        }

        [HttpGet]
        public IEnumerable<ContactReadDto> Get()
        {
            var contacts = _contacts.Find(c => true).ToList();
            return _mapper.Map<IEnumerable<ContactReadDto>>(contacts);
        }
    }
}
