using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Products
{
    public class ProductDto
    {
        public Guid ProductID { get; set; }
        public Guid CategoryID { get; set; }
        public string ProductName { get; set; }
        public double? PriceOld { get; set; }
        public double PriceNew { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
