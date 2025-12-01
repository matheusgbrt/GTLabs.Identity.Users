namespace GTLabs.Identity.Users.Domain.Users.Entities;

public interface IUser
{
    public string HashedPassword { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}