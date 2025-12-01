using ExternalDeps.Core.Dtos;
using ExternalDeps.Core.Enums;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using GTLabs.Identity.Users.Domain.Users.Entities;
using GTLabs.Identity.Users.Domain.Users.Models;
using GTLabs.Identity.Users.Domain.Users.Services;
using GTLabs.Identity.Users.Domain.Users.Validators;
using Gtlabs.Persistence.Extensions;
using Gtlabs.Persistence.Repository;
using Gtlabs.Redis.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GTLabs.Identity.Users.Host.Users.Services;

public class UserService : IUserService, ITransientDependency
{
    private readonly IRepository<User> _userRepository;
    private readonly ICacheService<CachedUser> _userCache;
    private readonly IUserValidationService _userValidationService;

    public UserService(IRepository<User> userRepository, ICacheService<CachedUser> cacheService, IUserValidationService userValidationService)
    {
        _userRepository = userRepository;
        _userCache = cacheService;
        _userValidationService = userValidationService;
    }
    
    
    public async Task<PagedEntityListSearchResult<UserOutput>> GetAll(PagedRequest pagedRequest)
    {
        var users = await _userRepository.Query().Page(pagedRequest.Page,pagedRequest.PageSize).ToListAsync();
        var count = await _userRepository.Query().CountAsync();
        return new PagedEntityListSearchResult<UserOutput>(){Entities = users.Select(users => users.ToOutput()), TotalCount = count};
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

    public async Task<EntityAlterationResult<UserOutput>> Create(UserCreation userCreation)
    {
        var searchResult = await GetUserByName(userCreation.Name);
        
        if (searchResult.Found)
        {
            return new EntityAlterationResult<UserOutput>(){Success = false, Entity = searchResult.Entity.ToOutput(), Error = EntityAlterationError.Conflict};
        }

        var user = new User(userCreation);
        var userForValidation = user.ForValidation();
        userForValidation.Password = userCreation.Password;
        
        var validation = _userValidationService
            .With<IUsernameValidation>()
            .With<IPasswordValidation>()
            .Validate(userForValidation);
        
        if (!validation.Success)
        {
            return new EntityAlterationResult<UserOutput>(){Success = false, Error = EntityAlterationError.ValidationError,ErrorMessage = validation.ErrorMessage};
        }
        
        await _userRepository.InsertAsync(user,true);
        return new EntityAlterationResult<UserOutput>(){Success = true, Entity = user.ToOutput()};
    }

    public async Task<EntityAlterationResult<UserOutput>> Delete(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new EntityAlterationResult<UserOutput>(){Success = false, Error = EntityAlterationError.NotFound};
        }

        if (user.IsDeleted)
        {
            return new EntityAlterationResult<UserOutput>(){Success = false, Entity = user.ToOutput(), Error = EntityAlterationError.Conflict};
        }
        await _userRepository.DeleteAsync(user,true);
        return new EntityAlterationResult<UserOutput>(){Success = true, Entity = user.ToOutput()};
    }

    public async Task<EntityAlterationResult<UserOutput>> Update(Guid userId, UserUpdate userUpdate)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new EntityAlterationResult<UserOutput>(){Success = false, Error = EntityAlterationError.NotFound};
        }
        var userForValidation = user.ForValidation();
        userForValidation.Password = userUpdate.Password;
        
        var validation = _userValidationService
            .With<IUsernameValidation>()
            .With<IPasswordValidation>()
            .Validate(userForValidation);
        
        if (!validation.Success)
        {
            return new EntityAlterationResult<UserOutput>(){Success = false, Error = EntityAlterationError.ValidationError,ErrorMessage = validation.ErrorMessage};
        }
        
        user.Update(userUpdate);
        await _userRepository.UpdateAsync(user,true);
        await _userCache.SetAsync(user.ToCachedUser(), TimeSpan.FromMinutes(30));
        return new EntityAlterationResult<UserOutput>(){Success = true, Entity = user.ToOutput()};
    }
    
    
}