using MusicalStore.Common.Enums;
using MusicalStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class Order : DateCommon
    {
        [Key]
        public Guid OrderID { get; set; }
        [ForeignKey("Id")]
        public string Id { get; set; }
        [Required]
        public string Receiver { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        public double TotalMoney { get; set; }


        public string? CreateBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime ModifiedDate { get; set; }


        public AppUser appUser { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
