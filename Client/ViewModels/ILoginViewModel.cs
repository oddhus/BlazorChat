using System.Threading.Tasks;

namespace BlazorChat.ViewModels
{
    public interface ILoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool LoadingLogin { get; set; }
        public Task Login();
    }
}