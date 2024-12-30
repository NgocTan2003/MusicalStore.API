using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Common.ResponseBase
{
    public class ChangePassword
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordOld { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,100}$",
        ErrorMessage = "Password must have at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
        public string PasswordNew { get; set; }
        [Compare("PasswordNew", ErrorMessage = "The PasswordNew and confirm PasswordNew do not match.")]
        public string ConfirmPasswordNew { get; set; }
    }
}
