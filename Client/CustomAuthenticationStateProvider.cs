using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorChat.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var currentUser = await _httpClient.GetFromJsonAsync<AccountDto>("accounts/me");

                if (currentUser != null && currentUser.UserId != null)
                {
                    //create a claim
                    var claimName = new Claim(ClaimTypes.Name, currentUser.Username);
                    var claimId = new Claim("UserId", currentUser.UserId);
                    //create claimsIdentity
                    var claimsIdentity = new ClaimsIdentity(new[] { claimName, claimId }, "serverAuth");
                    //create claimsPrincipal
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    return new AuthenticationState(claimsPrincipal);
                }
            }
            catch (System.Exception) { };
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}