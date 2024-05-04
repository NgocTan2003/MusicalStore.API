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
        public string FullName { get; set; } = "";
        [Required]
        public Gender Gender { get; set; }
        public string? Avatar { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpirationTime { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<FeedBack> FeedBacks { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
