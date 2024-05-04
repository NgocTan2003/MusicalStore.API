using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Products
{
    public class CreateProduct : DateCommon
    {
        [Required(ErrorMessage = ("CategoryID is required"))]
        public Guid CategoryID { get; set; }
        [Required(ErrorMessage = ("ProductName is required"))]
        public string ProductName { get; set; }
        [Required(ErrorMessage = ("PriceOld is required"))]
        [Range(1, double.MaxValue, ErrorMessage = "PriceOld must be greater than 0.")]
        public double PriceOld { get; set; }
        [Required(ErrorMessage = ("PriceNew is required"))]
        [Range(1, double.MaxValue, ErrorMessage = "PriceNew must be greater than 0.")]
        public double PriceNew { get; set; }
        [Required(ErrorMessage = ("Thumnail is required"))]
        public string Thumbnail { get; set; }
        [Required(ErrorMessage = ("Description is required"))]
        public string Description { get; set; }
        [Required(ErrorMessage = ("Quantity is required"))]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
