using DictionaryBack.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DictionaryBack.DAL
{
    public class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions options) : base(options) 
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Word> Words { get; set; }

        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasPostgresExtension("citext");
            // modelBuilder.HasCollation("ci_collation", locale: "en-u-ks-primary", provider: "icu", deterministic: false); 
        }
    }
}
