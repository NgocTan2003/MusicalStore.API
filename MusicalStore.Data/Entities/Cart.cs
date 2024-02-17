﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Data.Entities
{
    public class Cart
    {
        [Key]
        public Guid CartID { get; set; }
        [ForeignKey("UserID")]
        public Guid UserID { get; set; }


        public string? CreateBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    
    
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}