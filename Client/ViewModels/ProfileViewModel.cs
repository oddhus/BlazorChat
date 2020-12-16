using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class ProfileViewModel : IProfileViewModel
    {
        public string Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }

        public bool LoadingGet { get; set; }
        public bool LoadingUpdate { get; set; }

        private HttpClient _httpClient;

        public ProfileViewModel()
        {

        }

        public ProfileViewModel(HttpClient http)
        {
            _httpClient = http;
        }

        public static implicit operator ProfileViewModel(UserReadDto user)
        {
            return new ProfileViewModel
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Address = user.Address,
            };
        }

        public static implicit operator UserUpdateDto(ProfileViewModel user)
        {
            return new UserUpdateDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Address = user.Address,
            };
        }

        public async Task GetProfile()
        {
            LoadingGet = true;
            var profile = await _httpClient.GetFromJsonAsync<UserReadDto>("user/5fd731080232e18424df19ae");
            LoadCurrentObject(profile);
            LoadingGet = false;
        }

        public async Task UpdateProfile()
        {
            LoadingUpdate = true;
            await _httpClient.PutAsJsonAsync("user/5fd731080232e18424df19ae", this);
            LoadingUpdate = false;
        }

        private void LoadCurrentObject(ProfileViewModel p)
        {
            Firstname = p.Firstname;
            Lastname = p.Lastname;
            Address = p.Address;
        }
    }
}