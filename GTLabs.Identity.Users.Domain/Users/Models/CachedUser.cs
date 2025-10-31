using GTLabs.Identity.Users.Domain.Users.Entities;
using Gtlabs.Redis.Abstractions;

namespace GTLabs.Identity.Users.Domain.Users.Models;

public class CachedUser : CacheEntity
{
    public override string Prefix { get; } = "user";
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}
    