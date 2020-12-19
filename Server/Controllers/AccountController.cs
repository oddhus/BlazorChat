using Microsoft.AspNetCore.Mvc;
using BlazorChat.Shared.Dtos;
using AutoMapper;
using BlazorChat.Server.Services;
using BlazorChat.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

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
        public async Task<ActionResult<AccountDto>> LoginAccount([FromBody] LoginDto loginIn)
        {
            var account = _accountService.Login(loginIn);
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

            return await Task.FromResult(Ok(_mapper.Map<AccountDto>(account)));
        }
    }
}