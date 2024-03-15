using MusicalStore.Common.Enums;
using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Orders
{
    public class UpdateOrder : DateCommon
    {
        [Required]
        public Guid OrderID { get; set; }
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Receiver must be between 6 and 100 characters")]
        public string Receiver { get; set; }
        [RegularExpression(@"^\+?\d{10,12}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        [Required(ErrorMessage = ("TotalMoney is required"))]
        [Range(1, int.MaxValue, ErrorMessage = "TotalMoney must be greater than 0.")]
        public double TotalMoney { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
