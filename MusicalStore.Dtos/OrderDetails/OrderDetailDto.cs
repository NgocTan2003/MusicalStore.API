using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.OrderDetails
{
    public class OrderDetailDto
    {
        public Guid ProductID { get; set; }
        public Guid OrderID { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalDetails { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
