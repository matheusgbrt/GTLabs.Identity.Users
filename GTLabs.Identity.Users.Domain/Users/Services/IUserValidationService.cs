using GTLabs.Identity.Users.Domain.Users.DTOs;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Validators;

namespace GTLabs.Identity.Users.Domain.Users.Services;

public interface IUserValidationService
{
    IUserValidationService With<T>() where T : IUserValidator;
    UserValidationResult Validate(IUserForValidation user);
}