using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Categories
{
    public class CategoryDto
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
