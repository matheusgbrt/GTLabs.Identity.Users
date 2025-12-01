using GTLabs.Identity.Users.Domain.Users.DTOs;
using GTLabs.Identity.Users.Domain.Users.Entities;

namespace GTLabs.Identity.Users.Domain.Users.Validators;

public interface IUserValidator
{
    public UserValidationResult Validate(IUserForValidation user);
}