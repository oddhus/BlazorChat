using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class ProfileViewModel : IProfileViewModel
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool LoadingGet { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public bool isEditing { get; set; }
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

        public void Edit()
        {
            isEditing = !isEditing;
        }

        public async Task GetProfile()
        {
            LoadingGet = true;
            try
            {
                var profile = await _httpClient.GetFromJsonAsync<UserReadDto>("users/" + this.Id);
                LoadCurrentObject(profile);
                Failed = false;
                LoadingGet = false;
            }
            catch (System.Exception)
            {
                ErrorMessage = "Failed to get profile";
                Failed = true;
                LoadingGet = false;
            }
        }

        public async Task UpdateProfile()
        {
            LoadingUpdate = true;
            try
            {
                await _httpClient.PutAsJsonAsync("users/" + this.Id, this);
                Failed = false;
                isEditing = false;
                LoadingUpdate = false;
            }
            catch (System.Exception)
            {
                ErrorMessage = "Failed to update profile";
                Failed = true;
                LoadingUpdate = false;
            }
        }

        private void LoadCurrentObject(ProfileViewModel p)
        {
            Firstname = p.Firstname;
            Lastname = p.Lastname;
            Address = p.Address;
        }
    }
}