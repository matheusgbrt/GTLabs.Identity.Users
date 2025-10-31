using GTLabs.Identity.Users.Domain.Users.Entities;

namespace GTLabs.Identity.Users.Domain.Users.Models;

public class UserCreation
{
    public string Name { get; set; }
    public string Password { get; set; }
}