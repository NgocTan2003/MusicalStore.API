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
        [Required]
        public string PasswordNew { get; set; }
        [Compare("PasswordNew", ErrorMessage = "The PasswordNew and confirm PasswordNew do not match.")]
        public string ConfirmPasswordNew { get; set; }
    }
}
