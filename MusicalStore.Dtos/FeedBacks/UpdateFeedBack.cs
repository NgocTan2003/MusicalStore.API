using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.FeedBacks
{
    public class UpdateFeedback : DateCommon
    {
        [Required(ErrorMessage = "FeedBackID is required")]
        public Guid FeedBackID { get; set; }
        [Required(ErrorMessage = ("StarReview is required"))]
        [Range(1, int.MaxValue, ErrorMessage = "StarReivew must be greater than 0.")]
        public int StarReview { get; set; }
        public string? ContentRated { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
