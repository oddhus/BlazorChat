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
using System.Linq;

namespace BlazorChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountsController(ILogger<AccountsController> logger, AccountService accountService, IMapper mapper, UserService userService)
        {
            _accountService = accountService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet("me")]
        public ActionResult<AccountDto> Getme()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new AccountDto();
            }
            string id = User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            var account = _accountService.Get(id);
            return Ok(_mapper.Map<AccountDto>(account));
        }

        [HttpPost("register")]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] RegisterDto register)
        {
            if (_accountService.AccountExists(register.Username))
            {
                return null;
            }

            User user = new User();
            user = _mapper.Map<User>(register);
            _userService.CreateUser(user);
            Account account = _accountService.Register(user.Id, register);

            var claimName = new Claim(ClaimTypes.Name, account.Username);
            var claimAccount = new Claim("AccountId", account.Id);
            var clamUser = new Claim("UserId", account.UserId);

            var claimsIdentity = new ClaimsIdentity(new[] { claimName, claimAccount, clamUser }, "serverAuth");
            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //Sign In User
            await HttpContext.SignInAsync(claimsPrincipal);

            return await Task.FromResult(_mapper.Map<AccountDto>(account));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountDto>> LoginAccount([FromBody] LoginDto credentials)
        {
            _logger.LogInformation("User {Name} logged out at {Time}.", credentials.Username, DateTime.UtcNow);

            var account = _accountService.Login(credentials);
            if (account == null)
            {
                return StatusCode(403);
            }

            var claimName = new Claim(ClaimTypes.Name, account.Username);
            var claimAccount = new Claim("AccountId", account.Id);
            var clamUser = new Claim("UserId", account.UserId);
            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { claimName, claimAccount, clamUser }, "serverAuth");
            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //Sign In User
            await HttpContext.SignInAsync(claimsPrincipal);

            return await Task.FromResult(_mapper.Map<AccountDto>(account));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return NoContent();
        }
    }
}