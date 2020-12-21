using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

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

        [HttpGet("{userId}")]
        public ActionResult<UserReadDto> GetUser(string userId)
        {
            var user = _userService.Get(userId);
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(string userId, [FromBody] UserUpdateDto userIn)
        {
            var user = _userService.Get(userId);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Update(userId, user, userIn);
            return NoContent();
        }

        [HttpGet("settings/{userId}")]
        public ActionResult<UserSettingsDto> GetUserSettings(string userId)
        {
            var settings = _userService.GetUserSettings(userId);
            return Ok(_mapper.Map<UserSettingsDto>(settings));
        }

        [HttpPost("settings/{userId}")]
        public ActionResult UpdateUserSettings(string userId, [FromBody] UserSettingsDto settingIn)
        {
            var user = _userService.Get(userId);
            if (user == null)
            {
                return NotFound();
            }
            _userService.UpdateUserSettings(userId, settingIn);
            return NoContent();
        }

        [HttpGet("contacts")]
        public ActionResult<IEnumerable<ContactReadDto>> GetUserContacts()
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Forbid();
            }
            var user = _userService.GetUserContacts(userId);
            return Ok(_mapper.Map<IEnumerable<ContactReadDto>>(user.Contacts));
        }
    }
}
