namespace BlazorChat.Shared.Models
{
    public class UserReadDto
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        public UserReadDto(string firstname, string lastname, string address)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Address = address;
        }
    }
}