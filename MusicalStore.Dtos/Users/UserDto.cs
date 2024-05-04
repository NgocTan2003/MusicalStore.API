using Microsoft.AspNetCore.Identity;
using MusicalStore.Common.Enums;
using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Users
{
    public class UserDto : IdentityUser, DateCommon
    {
        public string FullName { get; set; } = "";
        public Gender Gender { get; set; }
        public string? Avatar { get; set; }
        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
