using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using GTLabs.Identity.Users.Domain.Users.DTOs;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Enums;

namespace GTLabs.Identity.Users.Domain.Users.Validators;

public class PasswordValidation : IPasswordValidation, ITransientDependency
{
    public UserValidationResult Validate(IUserForValidation user)
    {
        var error = UserValidation.InvalidPassword;
        var field = user.Password;
        if(field.Length < 5)
            return UserValidationResult.WithError(error,"Senha deve ter mais que 5 caracteres" );
        if (field.Length > 20)
            return UserValidationResult.WithError(error,"Senha deve ter menos que 20 caracteres");
        if (field.Contains(" "))
            return UserValidationResult.WithError(error,"Senha não deve conter espaços");
        
        return UserValidationResult.AsOk();
    }
}