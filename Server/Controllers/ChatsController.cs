using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using BlazorChat.Server.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return StatusCode(403);
            }

            var chat = _chatService.GetChat(chatId);
            if (chat == null || !(userId == chat.ParticipantA || userId == chat.ParticipantB))
            {
                return StatusCode(403);
            }

            return Ok(_mapper.Map<ChatDto>(chat));
        }

        [HttpPost("{userId}/start/{recipientId}")]
        public ActionResult<ContactDto> StartChat(string userId, string recipientId)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }

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
            var chat = _chatService.GetChat(chatId);
            if (!(HttpContext.User.HasClaim("UserId", chat.ParticipantA) || HttpContext.User.HasClaim("UserId", chat.ParticipantB)))
            {
                return StatusCode(403);
            }
            var sentMessage = _chatService.AddMessage(chatId, _mapper.Map<Message>(message));
            return Ok(_mapper.Map<MessageDto>(sentMessage));
        }
    }
}
