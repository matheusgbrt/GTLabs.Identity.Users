using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using GTLabs.Identity.Users.Domain.Users.DTOs;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GTLabs.Identity.Users.Domain.Users.Services;

public class UserValidationService : IUserValidationService, ITransientDependency
{
    private List<IUserValidator> _validators;
    private readonly IServiceProvider _serviceProvider;
    
    public UserValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _validators = new List<IUserValidator>();
    }
    
    public IUserValidationService With<T>() where T : IUserValidator
    {
        _validators.Add(_serviceProvider.GetRequiredService<T>());
        return this;
    }

    public UserValidationResult Validate(IUserForValidation user)
    {
        foreach (var validator in _validators)
        {
            var result = validator.Validate(user);
            if (!result.Success)
                return result;
        }
        return UserValidationResult.AsOk();
    }
}