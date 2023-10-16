using EVE_Industry.EfStuff.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVE_Industry.EfStuff
{
    public class WebContext : DbContext
    {
        public DbSet<MainIndustryCell> MainIndustryCells { get; set; }
        public DbSet<DumpCell> DumpCells { get; set; }

        public WebContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //   .HasMany(x => x.CellSuggestionsWhichIAprove)
            //   .WithOne(x => x.Approver);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
