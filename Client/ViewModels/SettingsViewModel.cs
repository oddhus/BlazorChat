using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        //properties
        public string Id { get; set; }
        public bool Notifications { get; set; }
        public bool DarkTheme { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool LoadingGet { get; set; }
        private HttpClient _httpClient;

        //methods
        public SettingsViewModel()
        {
        }
        public SettingsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task GetProfile()
        {
            LoadingGet = true;
            var settings = await _httpClient.GetFromJsonAsync<UserSettingsDto>("users/" + this.Id + "/settings");
            LoadCurrentObject(settings);
            LoadingGet = false;
        }
        public async Task Save()
        {
            LoadingUpdate = true;
            UserSettingsDto newSettings = this;
            var res = await _httpClient.PostAsJsonAsync("users/" + this.Id + "/settings", newSettings);
            LoadingUpdate = false;
        }
        public void DarkThemeChange()
        {
            DarkTheme = !DarkTheme;
        }
        public void NotificationsChange()
        {
            Notifications = !Notifications;
        }
        private void LoadCurrentObject(SettingsViewModel settingsViewModel)
        {
            DarkTheme = settingsViewModel.DarkTheme;
            Notifications = settingsViewModel.Notifications;
        }

        //operators
        public static implicit operator SettingsViewModel(UserSettingsDto user)
        {
            return new SettingsViewModel
            {
                Notifications = user.Notifications,
                DarkTheme = user.DarkTheme
            };
        }
        public static implicit operator UserSettingsDto(SettingsViewModel settingsViewModel)
        {
            return new UserSettingsDto
            {
                Notifications = settingsViewModel.Notifications,
                DarkTheme = settingsViewModel.DarkTheme
            };
        }
    }
}