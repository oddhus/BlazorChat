using System.Threading.Tasks;

namespace BlazorChat.ViewModels
{
    public interface ISettingsViewModel
    {
        public bool Notifications { get; set; }
        public bool DarkTheme { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool LoadingGet { get; set; }

        public Task Save();
        public Task GetProfile();
    }
}