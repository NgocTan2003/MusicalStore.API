using Microsoft.EntityFrameworkCore;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicalStore.Data.EF
{
    public static class DataSeeder
    {
        public static void Seed(this ModelBuilder modelBulder)
        {
            modelBulder.Entity<Role>().HasData(
                    new Role()
                    {
                        Id = 1,
                        Name = "Admin",
                        Description = "Administrator"
                    },
                    new Role()
                    {
                        Id = 2,
                        Name = "Manager",
                        Description = "Manager"
                    },
                    new Role()
                    {
                        Id = 3,
                        Name = "Customer",
                        Description = "Customer"
                    }
                );
        }
    }
}
