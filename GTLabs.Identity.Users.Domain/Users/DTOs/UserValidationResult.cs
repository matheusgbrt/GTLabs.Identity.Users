using GTLabs.Identity.Users.Domain.Users.Enums;

namespace GTLabs.Identity.Users.Domain.Users.DTOs;

public class UserValidationResult
{
    public bool Success => UserValidation == UserValidation.Ok;
    public UserValidation UserValidation { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    
    
    public static UserValidationResult AsOk() => new UserValidationResult(){UserValidation = UserValidation.Ok};
    public static UserValidationResult WithError(UserValidation userValidation, string errorMessage) => new UserValidationResult(){UserValidation = userValidation, ErrorMessage = errorMessage};
}