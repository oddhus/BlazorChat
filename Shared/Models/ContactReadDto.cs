

namespace BlazorChat.Shared.Models
{
    public class ContactReadDto
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        public ContactReadDto(string id, string firstname, string lastname, string address)
        {
            this.Id = id;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Address = address;
        }
    }
}