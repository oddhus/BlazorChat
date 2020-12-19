using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool LoadingLogin { get; set; }

        private HttpClient _httpClient;

        public LoginViewModel()
        {

        }

        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Login()
        {
            await _httpClient.PostAsJsonAsync<LoginDto>("users/login", this);
        }

        public static implicit operator LoginDto(LoginViewModel login)
        {
            return new LoginDto
            {
                Username = login.Username,
                Password = login.Password
            };
        }
    }
}