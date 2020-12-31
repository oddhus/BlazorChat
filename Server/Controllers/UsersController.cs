using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using System.Collections.Generic;
using BlazorChat.Server.Models;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ContactDto>> SearchUsers([FromQuery] string firstname, [FromQuery] string lastname)
        {
            var users = _userService.SearchUsers(firstname, lastname);
            return Ok(_mapper.Map<IEnumerable<ContactDto>>(users));
        }

        [HttpGet("{userId}")]
        public ActionResult<UserReadDto> GetUser(string userId)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var user = _userService.Get(userId);
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(string userId, [FromBody] UserUpdateDto userIn)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var user = _userService.Get(userId);
            if (user == null)
            {
                return StatusCode(404);
            }
            _userService.Update(userId, user, userIn);
            return NoContent();
        }

        [HttpGet("{userId}/settings")]
        public ActionResult<UserSettingsDto> GetUserSettings(string userId)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var settings = _userService.GetUserSettings(userId);
            return Ok(_mapper.Map<UserSettingsDto>(settings));
        }

        [HttpPost("{userId}/settings")]
        public ActionResult UpdateUserSettings(string userId, [FromBody] UserSettingsDto settingIn)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var user = _userService.Get(userId);
            if (user == null)
            {
                return StatusCode(404);
            }
            _userService.UpdateUserSettings(userId, settingIn);
            return NoContent();
        }

        [HttpGet("{userId}/contacts")]
        public ActionResult<IEnumerable<ContactDto>> GetUserContacts(string userId)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var user = _userService.GetUserContacts(userId);
            return Ok(_mapper.Map<IEnumerable<ContactDto>>(user.Contacts));
        }

        [HttpPost("{userId}/contacts")]
        public ActionResult<IEnumerable<ContactDto>> AddUserContacts(string userId, [FromBody] ContactDto contactDto)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var user = _userService.AddUserContacts(userId, _mapper.Map<Contact>(contactDto));
            return Ok(_mapper.Map<IEnumerable<ContactDto>>(user.Contacts));
        }

        [HttpDelete("{userId}/contacts/{contactId}")]
        public ActionResult DeleteContact(string userId, string contactId)
        {
            if (!HttpContext.User.HasClaim("UserId", userId))
            {
                return StatusCode(403);
            }
            var user = _userService.RemoveUserContacts(userId, contactId);
            return Ok();
        }
    }
}
