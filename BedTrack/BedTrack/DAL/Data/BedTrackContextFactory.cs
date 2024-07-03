using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


    namespace BedTrack.DAL.Data
    {
        public class BedTrackContextFactory : IDesignTimeDbContextFactory<BedTrackContext>
        {
            public BedTrackContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<BedTrackContext>();
                var connectionString = configuration.GetConnectionString("ConnectionString");
                optionsBuilder.UseSqlServer(connectionString);

                return new BedTrackContext(optionsBuilder.Options);
            }
        }
    }
