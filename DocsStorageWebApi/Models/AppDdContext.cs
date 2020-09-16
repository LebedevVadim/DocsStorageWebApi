using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsStorageWebApi.Models
{
    public class AppDdContext : DbContext
    {
        public AppDdContext(DbContextOptions<AppDdContext> options) : base (options)
        {

        }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().HasIndex(x => new { x.DocumentID, x.DocumentName, x.LoadDate });
            modelBuilder.Entity<Document>().HasIndex(x => x.DocumentStorageID).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
