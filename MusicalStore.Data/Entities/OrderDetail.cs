using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class OrderDetail : DateCommon
    {
        [Key]
        [ForeignKey("ProductID")]
        public Guid ProductID { get; set; }

        [Key]
        [ForeignKey("OrderID")]
        public Guid OrderID { get; set; }

        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double TotalDetails { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }



        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
