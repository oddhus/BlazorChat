using BlazorChat.Shared.Models;

namespace BlazorChat.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }

        public static implicit operator ProfileViewModel(UserReadDto user)
        {
            return new ProfileViewModel
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Address = user.Address,
            };
        }
    }
}