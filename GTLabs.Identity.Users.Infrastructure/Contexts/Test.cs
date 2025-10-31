using Gtlabs.Api.AmbientData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GTLabs.Identity.Users.Infrastructure.Contexts
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            // Try to get connection string from environment variable
            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            // Dummy IAmbientData for design-time
            IAmbientData ambientData = new DesignTimeAmbientData();

            return new IdentityDbContext(optionsBuilder.Options, ambientData);
        }

        private class DesignTimeAmbientData : IAmbientData
        {
            public Guid? GetUserId() => Guid.Empty;
        }
    }
}