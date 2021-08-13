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
            using (var scope = host.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DAL.Seeder>();
                seeder.MigrateAndSeed(File.ReadAllText($"Static{Path.DirectorySeparatorChar}serializeddict.json"));
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //.UseUrls("http://localhost:61598");
                });
    }
}
