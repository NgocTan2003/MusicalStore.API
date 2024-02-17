using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class CartItem
    {
        [Key]
        [Column(Order = 0)]
        //[ForeignKey("Cart")]
        public Guid CartID { get; set; }

        [Key]
        [Column(Order = 1)]
        //[ForeignKey("Product")]
        public Guid ProductID { get; set; }

        public int Quantity { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }


        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
