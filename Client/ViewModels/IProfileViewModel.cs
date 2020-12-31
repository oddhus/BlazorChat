using System.Threading.Tasks;

namespace BlazorChat.ViewModels
{
    public interface IProfileViewModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool LoadingGet { get; set; }
        public bool LoadingUpdate { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public bool isEditing { get; set; }
        public void Edit();
        public Task GetProfile();
        public Task UpdateProfile();
    }
}