using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 100 characters")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
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

        public string? Avatar { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<FeedBack> FeedBacks { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
