using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.OrderDetails
{
    public class CreateOrderDetail: DateCommon
    {
        [Required(ErrorMessage ="ProductID is required ")]
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = "OrderID is required ")]
        public Guid OrderID { get; set; }
        [Required(ErrorMessage = "UnitPrice is required ")]
        [Range(1, double.MaxValue, ErrorMessage = "UnitPrice must be greater than 0.")]
        public double UnitPrice { get; set; }
        [Required(ErrorMessage = "Quantity is required ")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "TotalDetails is required ")]
        [Range(1, double.MaxValue, ErrorMessage = "TotalDetails must be greater than 0.")]
        public double TotalDetails { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
