using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }


        public ICollection<Product> Products { get; set; }

    }
}
