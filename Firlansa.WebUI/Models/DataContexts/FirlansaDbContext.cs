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

    }
}
