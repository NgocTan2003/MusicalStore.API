using MusicalStore.Common.Enums;
using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Users
{
    public class RegisterRequest : DateCommon
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 100 characters")]
        public string UserName { get; set; } = "";
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,100}$",
        ErrorMessage = "Password must have at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
        public string PassWord { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "FullName is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "FullName must be between 6 and 100 characters")]
        public string FullName { get; set; } = "";
        [Required]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?\d{10,12}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = "";
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
