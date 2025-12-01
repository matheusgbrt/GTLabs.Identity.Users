using ExternalDeps.Core.Dtos;
using ExternalDeps.Core.Enums;
using Gtlabs.Api.AmbientData;
using GTLabs.Identity.Users.Domain.Users.Models;
using GTLabs.Identity.Users.Host.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace GTLabs.Identity.Users.Host.Users.Controllers;

[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAmbientData _ambientData;

    public UsersController(IUserService userService, IAmbientData ambientData)
    {
        _userService = userService;
        _ambientData = ambientData;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll([FromQuery] PagedRequest pagedRequest)
    {
        var searchResult = await _userService.GetAll(pagedRequest);
        return Ok(searchResult);
    }

    [HttpGet]
    [Route("me")]
    public async Task<IActionResult> Me()
    {
        var userId = _ambientData.GetUserId();
        var searchResult = await _userService.GetUserById(userId!.Value);
        if (!searchResult.Found)
            return NotFound();

        return Ok(searchResult.Entity);
    }
    
    [HttpGet]
    [Route("{userID:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid userID)
    {
        var searchResult = await _userService.GetUserById(userID);
        if (!searchResult.Found)
            return NotFound();

        return Ok(searchResult.Entity);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] UserCreation userCreation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entityChangeResult = await _userService.Create(userCreation);
        if (!entityChangeResult.Success)
        {
            switch (entityChangeResult.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict();
                case EntityAlterationError.NotFound:
                    return NotFound();
                case EntityAlterationError.ValidationError:
                    return BadRequest(entityChangeResult);
            }
        }

        return Ok(entityChangeResult.Entity);
    }

    [HttpPut]
    [Route("{userID:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UserUpdate userUpdate)
    {
        var entityChangeResult = await _userService.Update(userId,userUpdate);
        if (!entityChangeResult.Success)
        {
            switch (entityChangeResult.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict();
                case EntityAlterationError.ValidationError:
                    return BadRequest(entityChangeResult);
            }
        }
        return Ok(entityChangeResult.Entity);
    }

    [HttpDelete]
    [Route("{userID:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var entityChangeResult = await _userService.Delete(userId);
        if (!entityChangeResult.Success)
        {
            switch (entityChangeResult.Error)
            {
                case EntityAlterationError.NotFound:
                    return NotFound();
                case EntityAlterationError.Conflict:
                    return Conflict();
            }
        }
        return Ok();
    }
    

}