using System.Threading.Tasks;

namespace BlazorChat.ViewModels
{
    public interface IRegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool DarkTheme { get; set; }
        public bool Notifications { get; set; }
        public bool LoadingOnSubmit { get; set; }
        public Task Register();
    }
}