using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        [ForeignKey("FunctionId")]
        public Guid FunctionId { get; set; }
        [Required]
        public bool CanCreate { set; get; }
        [Required]
        public bool CanRead { set; get; }
        [Required]
        public bool CanUpdate { set; get; }
        [Required]
        public bool CanDelete { set; get; }

        public Function Function { get; set; }

    }
}
