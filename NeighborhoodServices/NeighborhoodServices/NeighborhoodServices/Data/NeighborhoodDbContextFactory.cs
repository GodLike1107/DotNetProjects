using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NeighborhoodServices.API.Data
{
    public class NeighborhoodDbContextFactory : IDesignTimeDbContextFactory<NeighborhoodDbContext>
    {
        public NeighborhoodDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Needed for migrations
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<NeighborhoodDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new NeighborhoodDbContext(optionsBuilder.Options);
        }
    }
}
