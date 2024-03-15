using System.ComponentModel.DataAnnotations;

namespace MusicalStore.Dtos.Users
{
    public class AuthenticationRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
