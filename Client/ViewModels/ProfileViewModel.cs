using BlazorChat.Shared.Models;

namespace BlazorChat.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string EmailAddress { get; set; }

        public static implicit operator ProfileViewModel()
        {
            return new ProfileViewModel
            {

            }
        }
    }
}