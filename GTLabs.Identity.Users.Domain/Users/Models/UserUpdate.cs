using GTLabs.Identity.Users.Domain.Users.Entities;

namespace GTLabs.Identity.Users.Domain.Users.Models;

public class UserUpdate
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;  
    public bool IsActive { get; set; }
}