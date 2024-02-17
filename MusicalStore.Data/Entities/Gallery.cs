using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class Gallery
    {
        [Key]
        public Guid GalleryID { get; set; }
        [ForeignKey("ProductID")]
        public Guid ProductID { get; set; }
        public string? Thumbnail { get; set; }

        
        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Product Product { get; set; }
    }
}
