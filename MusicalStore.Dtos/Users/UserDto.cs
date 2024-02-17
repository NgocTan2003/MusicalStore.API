using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Users
{
    public class UserDto
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; } = "";
        public string PassWord { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string? Avatar { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
