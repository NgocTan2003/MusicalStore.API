using Microsoft.AspNetCore.Identity;
using MusicalStore.Common.Enums;
using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class AppUser : IdentityUser, DateCommon
    {
        //[Key]
        //public Guid UserID { get; set; }
        //[Required(ErrorMessage = "Username is required")]
        //public string UserName { get; set; } = "";
        //[Required(ErrorMessage = "Password is required")]
        //public string PassWord { get; set; } = "";
        //[Required(ErrorMessage = "Email is required")]
        //public string Email { get; set; } = "";
        //[Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; } = "";
        [Required]
        public Gender Gender { get; set; }
        //[Required(ErrorMessage = "Phone number is required")]
        //public string PhoneNumber { get; set; } = "";
        public string? Avatar { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<FeedBack> FeedBacks { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
