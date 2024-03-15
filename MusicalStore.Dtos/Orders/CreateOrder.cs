using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicalStore.Common.Enums;

namespace MusicalStore.Dtos.Orders
{
    public class CreateOrder : DateCommon
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Receiver must be between 6 and 100 characters")]
        public string Receiver { get; set; }
        [Required]
        [RegularExpression(@"^\+?\d{10,12}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required(ErrorMessage = ("TotalMoney is required"))]
        [Range(1, double.MaxValue, ErrorMessage = "TotalMoney must be greater than 0.")]
        public double TotalMoney { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
