using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenteApi.Models
{
    public class VenteContext : DbContext
    {
        public VenteContext(DbContextOptions<VenteContext> options)
            : base(options)
        {
        }

        public DbSet<VenteItem> ProduitItems { get; set; }
    }
}
