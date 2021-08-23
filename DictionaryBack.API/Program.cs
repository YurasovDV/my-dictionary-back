using DictionaryBack.BL.Seeding;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DictionaryBack.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            MigrateAndSeed(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        private static void MigrateAndSeed(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<Seeder>();
                seeder.Migrate();

                var serializedDict = File.ReadAllText($"Static{Path.DirectorySeparatorChar}serializeddict.json");
                seeder.Seed(serializedDict);
            }
        }

        // in case if full db recreation is needed
        private static void MigrateAndSeedFromScratch(IHost host)
        {
            // https://github.com/npgsql/npgsql/issues/2366

            bool isDevelopment = false;

            using (var scope = host.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<Seeder>();
                var hostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                isDevelopment = hostEnvironment.IsDevelopment();
                if (isDevelopment)
                {
                    // step 1: drop db
                    seeder.DropDatabase();
                }
                else
                {
                    // no database dropping
                    seeder.Migrate();
                }
            }

            // step 2: refresh types
            // hack: create new scope so pg conn pool refreshes all caches
            // won't work with dbContextPool
            if (isDevelopment)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<Seeder>();
                    seeder.Migrate();
                }
            }

            // step 3: seed data after types are reloaded
            using (var scope = host.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<Seeder>();
                var serializedDict = File.ReadAllText($"Static{Path.DirectorySeparatorChar}serializeddict.json");
                seeder.Seed(serializedDict);
            }
        }
    }
}
