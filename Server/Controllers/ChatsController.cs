using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using BlazorChat.Server.Models;
using System.Collections.Generic;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ChatsController(ChatService chatservice, UserService userService, IMapper mapper)
        {
            _chatService = chatservice;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet("{chatId}")]
        public ActionResult<ChatDto> GetMessages(string chatId)
        {
            return Ok(_mapper.Map<ChatDto>(_chatService.GetChat(chatId)));
        }

        [HttpPost("{userId}/start/{recipientId}")]
        public ActionResult<ContactDto> StartChat(string userId, string recipientId)
        {
            var chat = _chatService.GetChat(userId, recipientId);
            if (chat == null)
            {
                chat = _chatService.StartChat(userId, recipientId);
            }
            var contacts = _userService.GetUserContacts(userId).Contacts;
            var contact = contacts.Find(c => c.Id == recipientId);
            contact.ChatId = chat.Id;
            _userService.UpdateUserContacts(userId, recipientId, chat.Id);
            return _mapper.Map<ContactDto>(contact);
        }

        [HttpPost("{chatId}/message")]
        public ActionResult<MessageDto> AddMessage(string chatId, [FromBody] MessageDto message)
        {
            var sentMessage = _chatService.AddMessage(chatId, _mapper.Map<Message>(message));
            return Ok(_mapper.Map<MessageDto>(sentMessage));
        }
    }
}
