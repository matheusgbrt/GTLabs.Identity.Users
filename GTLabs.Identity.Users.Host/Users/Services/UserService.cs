using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Models;
using GTLabs.Identity.Users.Host.Consts;
using GTLabs.Identity.Users.Host.Dtos;
using Gtlabs.Persistence.Repository;
using Gtlabs.Redis.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GTLabs.Identity.Users.Host.Users.Services;

public class UserService : IUserService, ITransientDependency
{
    private readonly IRepository<User> _userRepository;
    private readonly ICacheService<CachedUser> _userCache;

    public UserService(IRepository<User> userRepository, ICacheService<CachedUser> cacheService)
    {
        _userRepository = userRepository;
        _userCache = cacheService;
    }


    public async Task<EntitySearchResult<User>> GetUserById(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new EntitySearchResult<User>(){Found = false};
        }
        await _userCache.SetAsync(user.ToCachedUser(), TimeSpan.FromMinutes(30));
        return new EntitySearchResult<User>(){Found = true, Entity = user};
    }
    
    public async Task<EntitySearchResult<User>> GetUserByName(string username)
    {
        var user = await _userRepository.Query().Where(u => u.Name.ToLower() == username.ToLower()).FirstOrDefaultAsync();
        if (user == null)
        {
            return new EntitySearchResult<User>(){Found = false};
        }
        
        return new EntitySearchResult<User>(){Found = true, Entity = user};
        
    }

    public async Task<EntityAlterationResult<User>> Create(UserCreation userCreation)
    {
        var searchResult = await GetUserByName(userCreation.Name);
        if (searchResult.Found)
        {
            return new EntityAlterationResult<User>(){Success = false, Entity = searchResult.Entity, Error = EntityAlterationError.Conflict};
        }

        var user = new User(userCreation);
        await _userRepository.InsertAsync(user,true);
        return new EntityAlterationResult<User>(){Success = true, Entity = user};
    }

    public async Task<EntityAlterationResult<User>> Delete(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new EntityAlterationResult<User>(){Success = false, Error = EntityAlterationError.NotFound};
        }

        if (user.IsDeleted)
        {
            return new EntityAlterationResult<User>(){Success = false, Entity = user, Error = EntityAlterationError.Conflict};
        }
        await _userRepository.DeleteAsync(user,true);
        return new EntityAlterationResult<User>(){Success = true, Entity = user};
    }

    public async Task<EntityAlterationResult<User>> Update(Guid userId, UserUpdate userUpdate)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new EntityAlterationResult<User>(){Success = false, Error = EntityAlterationError.NotFound};
        }
        user.Update(userUpdate);
        await _userRepository.UpdateAsync(user,true);
        await _userCache.SetAsync(user.ToCachedUser(), TimeSpan.FromMinutes(30));
        return new EntityAlterationResult<User>(){Success = true, Entity = user};
    }
    
    
}