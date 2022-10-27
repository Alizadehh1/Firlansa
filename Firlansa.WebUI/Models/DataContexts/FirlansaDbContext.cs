using Firlansa.WebUI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.DataContexts
{
    public class FirlansaDbContext : DbContext
    {
        public FirlansaDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
    }
}
