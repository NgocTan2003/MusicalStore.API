using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }
        [ForeignKey("CategoryID")]
        public Guid CategoryID { get; set; }
        [Required]
        public string ProductName { get; set; }
        public double? PriceOld { get; set; }
        [Required]
        public double PriceNew { get; set; }
        [Required]
        public string Thumbnail { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }


        public Category Category { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<FeedBack> FeedBacks { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<CartItem> CartItems { get; set; }


    }
}
