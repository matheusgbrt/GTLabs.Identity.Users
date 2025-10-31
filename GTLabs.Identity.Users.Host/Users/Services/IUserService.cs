using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Models;
using GTLabs.Identity.Users.Host.Dtos;
using Gtlabs.Persistence.Repository;

namespace GTLabs.Identity.Users.Host.Users.Services;

public interface IUserService
{
    Task<EntitySearchResult<User>> GetUserById(Guid userId);
    Task<EntitySearchResult<User>> GetUserByName(string username);
    Task<EntityAlterationResult<User>> Create(UserCreation userCreation);
    Task<EntityAlterationResult<User>> Delete(Guid userId);
    Task<EntityAlterationResult<User>> Update(Guid userId, UserUpdate userUpdate);

}