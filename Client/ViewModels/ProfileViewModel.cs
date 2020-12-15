using BlazorChat.Shared.Dtos;

namespace BlazorChat.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }

        public string Message { get; set; }

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
    }
}