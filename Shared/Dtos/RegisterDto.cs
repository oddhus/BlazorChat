using System.ComponentModel.DataAnnotations;

namespace BlazorChat.Shared.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool DarkTheme { get; set; }
        public bool Notifications { get; set; }
    }
}