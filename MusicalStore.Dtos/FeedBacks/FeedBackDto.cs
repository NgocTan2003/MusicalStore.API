using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.FeedBacks
{
    public class FeedBackDto : DateCommon
    {
        public Guid FeedBackID { get; set; }
        public string Id { get; set; }
        public Guid ProductID { get; set; }
        public int StarReview { get; set; }
        public string? ContentRated { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
