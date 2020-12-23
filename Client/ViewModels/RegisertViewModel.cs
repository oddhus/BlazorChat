using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class RegisterViewModel : IRegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RepeatPassword { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool DarkTheme { get; set; }
        public bool Notifications { get; set; }
        public bool LoadingOnSubmit { get; set; }
        private HttpClient _httpClient;

        public RegisterViewModel() { }

        public RegisterViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Register()
        {
            RegisterDto register = this;
            var response = await _httpClient.PostAsJsonAsync<RegisterDto>("accounts/register", register);
        }

        public static implicit operator RegisterDto(RegisterViewModel register)
        {
            return new RegisterDto
            {
                Username = register.Username,
                Password = register.Password,
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                Address = register.Address,
                DarkTheme = register.DarkTheme,
                Notifications = register.Notifications
            };
        }
    }
}