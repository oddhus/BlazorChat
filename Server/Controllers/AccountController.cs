using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using Microsoft.Extensions.Logging;
using BlazorChat.Server.Models;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly AccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(AccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("me/{userId}")]
        public ActionResult<UserReadDto> GetMe(string userId)
        {
            var user = _accountService.Get(userId);
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPost("create")]
        public IActionResult CreateAccount([FromBody] RegisterDto register)
        {
            _accountService.Register(_mapper.Map<Account>(register));
            return NoContent();
        }

        [HttpPost("login")]
        public ActionResult LoginAccount([FromBody] LoginDto loginIn)
        {
            var account = _accountService.Login(loginIn);
            if (account == null)
            {
                return Forbid();
            }
            return NoContent();
        }
    }
}