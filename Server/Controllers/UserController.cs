using System;
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
    public class UserController : ControllerBase
    {

        private IMongoCollection<User> _users;
        private readonly IMapper _mapper;


        public UserController(IMongoClient client, IMapper mapper)
        {
            _mapper = mapper;
            var database = client.GetDatabase("BlazorChat");
            _users = database.GetCollection<User>("users");
        }

        [HttpGet("{userId}")]
        public UserReadDto Get(string userId)
        {
            var user = _users.Find<User>(u => u.Id == userId).FirstOrDefault();
            return _mapper.Map<UserReadDto>(user);
        }
    }
}
