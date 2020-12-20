using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using BlazorChat.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountsController(ILogger<AccountsController> logger, AccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("me")]
        public ActionResult<AccountDto> GetMe()
        {
            AccountDto account = new AccountDto();
            if (User.Identity.IsAuthenticated)
            {
                account.Username = User.FindFirstValue(ClaimTypes.Name);
            }
            return Ok(account);
        }

        [HttpPost("create")]
        public IActionResult CreateAccount([FromBody] RegisterDto register)
        {
            _accountService.Register(_mapper.Map<Account>(register));
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountDto>> LoginAccount([FromBody] LoginDto credentials)
        {
            _logger.LogInformation("User {Name} logged out at {Time}.", credentials.Username, DateTime.UtcNow);

            var account = _accountService.Login(credentials);
            if (account == null)
            {
                return Forbid();
            }

            var claim = new Claim(ClaimTypes.Name, account.Username);
            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //Sign In User
            await HttpContext.SignInAsync(claimsPrincipal);

            return await Task.FromResult(_mapper.Map<AccountDto>(account));
        }
    }
}