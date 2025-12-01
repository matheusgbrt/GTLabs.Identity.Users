using ExternalDeps.Core.Hashing;
using GTLabs.Identity.Users.Domain.Users.Models;
using Gtlabs.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace GTLabs.Identity.Users.Domain.Users.Entities;

public class User : AuditedEntity, IUser
{
    public string Name { get; set; }
    public string HashedPassword { get; set; }
    public bool IsActive { get; set; }

    //EF-Core
    public User()
    {
        
    }
    public User(UserCreation userCreation)
    {
        Name = userCreation.Name;
        IsActive = true;
        HashedPassword = PasswordHasher.HashPassword(userCreation.Password);
    }

    public void Update(UserUpdate userUpdate)
    {
        Name = userUpdate.Name;
        IsActive = userUpdate.IsActive;
        HashedPassword = PasswordHasher.HashPassword(userUpdate.Password);
    }

    public CachedUser ToCachedUser()
    {
        return new CachedUser()
        {
            Id = Id,
            Name = Name,
            IsActive = IsActive,
        };
    }
    
    public UserOutput ToOutput()
    {
        return new UserOutput()
        {
            Id = Id,
            Name = Name,
            IsActive = IsActive,
            CreationTime = CreationTime,
            LastModificationTime = LastModificationTime,
            CreatorId = CreatorId,
            ModifierId = ModifierId,
        };
    }

    public UserForValidation ForValidation()
    {
        return new UserForValidation()
        {
            Name = Name
        };
    }
}