using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
