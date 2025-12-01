namespace GTLabs.Identity.Users.Domain.Users.Entities;

public interface IUserForValidation
{
    public string Name { get; set; }
    public string Password { get; set; }
}