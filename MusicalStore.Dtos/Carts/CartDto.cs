using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Carts
{
    public class CartDto
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
