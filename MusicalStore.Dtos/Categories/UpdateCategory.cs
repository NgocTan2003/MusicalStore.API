using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Categories
{
    public class UpdateCategory
    {
        [Required(ErrorMessage = ("CategoryID is required"))]
        public Guid CategoryID { get; set; }
        [Required(ErrorMessage = ("CategoryName is required"))]
        public string CategoryName { get; set; }

        public string? CreateBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
