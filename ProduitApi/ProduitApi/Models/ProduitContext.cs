using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ProduitApi.Models
{
    public class ProduitContext : DbContext
    {
        public ProduitContext(DbContextOptions<ProduitContext> options)
            : base(options)
        {
        }

        public DbSet<ProduitItem> ProduitItems { get; set; }
    }
}
