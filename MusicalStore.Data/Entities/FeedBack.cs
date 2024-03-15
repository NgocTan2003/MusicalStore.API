using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class FeedBack : DateCommon
    {
        [Key]
        public Guid FeedBackID { get; set; }
        [ForeignKey("Id")]
        public string Id { get; set; }
        [ForeignKey("ProductID")]
        public Guid ProductID { get; set; }
        [Required]
        public int StarReview { get; set; }
        public string? ContentRated { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }


        public AppUser appUser { get; set; }
        public Product Product { get; set; }
    }
}
