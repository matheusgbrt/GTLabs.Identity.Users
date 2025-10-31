namespace GTLabs.Identity.Users.Domain.Users.Entities;

public interface IUser
{
    public string Name { get; set; }
    public string HashedPassword { get; set; }
    public bool IsActive { get; set; }
}