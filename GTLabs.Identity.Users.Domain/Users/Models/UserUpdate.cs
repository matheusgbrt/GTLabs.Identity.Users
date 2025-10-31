namespace GTLabs.Identity.Users.Domain.Users.Models;

public class UserUpdate
{
    public string Name { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
}