using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicalStore.Common.Enums;

namespace MusicalStore.Dtos.FeedBacks
{
    public class CreateFeedback : DateCommon
    {
        [Required(ErrorMessage = "UserID is required")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Product is required")]
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = ("StarReview is required"))]
        [Range(1, int.MaxValue, ErrorMessage = "StarReview must be greater than 0.")]
        public int StarReview { get; set; }
        public string? ContentRated { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
