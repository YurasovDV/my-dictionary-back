using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DictionaryBack.DAL
{
    public class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions options) : base(options) { }

        public DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
