using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.Dtos
{
    public class RegisterDto
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
