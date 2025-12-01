using System.Text.RegularExpressions;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using GTLabs.Identity.Users.Domain.Users.DTOs;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Enums;

namespace GTLabs.Identity.Users.Domain.Users.Validators;

public class UsernameValidation : IUsernameValidation, ITransientDependency
{
    private static readonly Regex AllowedPattern =
        new Regex(@"^[a-z0-9\-]+$", RegexOptions.Compiled);

    public UserValidationResult Validate(IUserForValidation user)
    {
        var error = UserValidation.InvalidUsername;
        var userName = user.Name;

        if (string.IsNullOrWhiteSpace(userName))
            return UserValidationResult.WithError(error, "Nome de usuário não pode ser vazio.");

        if (userName.Length < 5)
            return UserValidationResult.WithError(error, "Nome de usuário deve ter mais que 5 caracteres.");

        if (userName.Length > 20)
            return UserValidationResult.WithError(error, "Nome de usuário deve ter menos que 20 caracteres.");

        if (userName.Contains(" "))
            return UserValidationResult.WithError(error, "Nome de usuário não deve conter espaços.");

        if (!AllowedPattern.IsMatch(userName))
            return UserValidationResult.WithError(
                error,
                "Nome de usuário deve conter apenas letras minúsculas, números e hífen (-)."
            );

        return UserValidationResult.AsOk();
    }
}