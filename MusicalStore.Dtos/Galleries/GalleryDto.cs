using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Galleries
{
    public class GalleryDto : DateCommon
    {
        public Guid GalleryID { get; set; }
        public Guid ProductID { get; set; }
        public string? Thumbnail { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
