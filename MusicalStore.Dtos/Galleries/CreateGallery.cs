using MusicalStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.Galleries
{
    public class CreateGallery : DateCommon
    {
        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductID { get; set; }
        public string? Thumbnail { get; set; }
        public UploadFile? UploadFile { get; set; }

        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
