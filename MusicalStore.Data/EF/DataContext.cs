﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicalStore.Data.Entities;

namespace MusicalStore.Data.EF
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartID, ci.ProductID });

            modelBuilder.Entity<OrderDetail>()
                .HasKey(ci => new { ci.OrderID, ci.ProductID });

            base.OnModelCreating(modelBuilder);
        }
    }
}
