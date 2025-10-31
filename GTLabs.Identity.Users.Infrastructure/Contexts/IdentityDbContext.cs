using Gtlabs.Api.AmbientData;
using GTLabs.Identity.Users.Domain.Users.Entities;
using Gtlabs.Persistence.CustomDbContext;
using Microsoft.EntityFrameworkCore;

namespace GTLabs.Identity.Users.Infrastructure.Contexts;

public class IdentityDbContext : GtLabsDbContext
{
    public DbSet<User> Users { get; set; }
    public IdentityDbContext(DbContextOptions options, IAmbientData ambientData) : base(options, ambientData)
    {
    }
    
    public class IdentityDbContextFactory : GtLabsDbContextFactory<IdentityDbContext>
    {
    }
}