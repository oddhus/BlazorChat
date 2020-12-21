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
            UserReadDto currentUser = await _httpClient.GetFromJsonAsync<UserReadDto>("users/me");

            if (currentUser != null && currentUser.Id != null)
            {
                //create a claim
                var claimName = new Claim(ClaimTypes.Name, currentUser.Firstname);
                var claimId = new Claim("UserId", currentUser.Id);
                //create claimsIdentity
                var claimsIdentity = new ClaimsIdentity(new[] { claimName, claimId }, "serverAuth");
                //create claimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}