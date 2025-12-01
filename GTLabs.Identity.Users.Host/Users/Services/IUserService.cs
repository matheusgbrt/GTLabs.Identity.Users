using Gtlabs.Core.Dtos;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Models;

namespace GTLabs.Identity.Users.Host.Users.Services;

public interface IUserService
{
    Task<EntitySearchResult<User>> GetUserById(Guid userId);
    Task<PagedEntityListSearchResult<UserOutput>> GetAll(PagedRequest pagedRequest);
    Task<EntitySearchResult<User>> GetUserByName(string username);
    Task<EntityAlterationResult<UserOutput>> Create(UserCreation userCreation);
    Task<EntityAlterationResult<UserOutput>> Delete(Guid userId);
    Task<EntityAlterationResult<UserOutput>> Update(Guid userId, UserUpdate userUpdate);

}