using GTLabs.Identity.Users.Domain.Users.Entities;
using Gtlabs.Redis.Abstractions;

namespace GTLabs.Identity.Users.Domain.Users.Models;

public class CachedUser : CacheEntity
{
    public override string Prefix { get; } = "user";
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
    