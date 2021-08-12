using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DictionaryBack.DAL
{
    internal class DesignDictionaryContextFactory : IDesignTimeDbContextFactory<DictionaryContext>
    {
        public DictionaryContext CreateDbContext(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Database=dict;Username=postgres;Password=1221");

            return new DictionaryContext(dbContextOptionsBuilder.Options);
        }
    }
}
