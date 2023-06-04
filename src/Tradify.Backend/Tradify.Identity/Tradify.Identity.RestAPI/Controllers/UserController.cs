using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Common.Extensions;
using Tradify.Identity.Application.Features.User.Commands;
using Tradify.Identity.Application.Features.User.Queries;
using Tradify.Identity.RestAPI.Models;

namespace Tradify.Identity.RestAPI.Controllers;

[Route("user")]
public class UserController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel requestModel)
    {
        var request = Mapper.Map<RegisterCommand>(requestModel);

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            userAlreadyExists => Conflict(userAlreadyExists),
            validationResult => BadRequest(validationResult.ToProblemDetails()));
    }

    // [HttpGet("public/{usersIds}")]
    // public async Task<IActionResult> GetUsersByIds([FromRoute] IEnumerable<long> usersIds)
    // {
    //     var request = new GetUsersSummariesQuery()
    //     {
    //         UsersIds = usersIds
    //     };
    //
    //     var result = await Mediator.Send(request);
    //     return result.Match<IActionResult>(
    //         success => Ok(success.Value),
    //         userNotFound => NotFound(userNotFound));
    // }
    
    [HttpGet("public/{userId:int}")]
    public async Task<IActionResult> GetUserProfileById([FromRoute] long userId)
    {
        var request = new GetUserProfileQuery()
        {
            UserId = userId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            model => Ok(model),
            userNotFound => NotFound(userNotFound));
    }
    
    [Authorize]
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserPersonalById([FromRoute] long userId)
    {
        var request = new GetUserPersonalQuery()
        {
            UserId = userId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            model => Ok(model),
            userNotFound => NotFound(userNotFound));
    }
    
    // [Authorize]
    // [HttpPut("{userId:int}")]
    // public async Task<IActionResult> UpdateUserData([FromBody] UpdateUserDataRequestModel updateUserDataRequestModel)
    // {
    //     var request = new UpdateUserDataCommand()
    //     {
    //         UserId = updateUserDataRequestModel.UserId, // TODO: take Id out of here ( receive id from Claims )
    //         AvatarPath = updateUserDataRequestModel.AvatarPath,
    //         FirstName = updateUserDataRequestModel.FirstName,
    //         LastName = updateUserDataRequestModel.LastName,
    //         MiddleName = updateUserDataRequestModel.MiddleName,
    //         Phone = updateUserDataRequestModel.Phone,
    //         HomeAddress = updateUserDataRequestModel.HomeAddress,
    //         BirthDate = updateUserDataRequestModel.BirthDate
    //     };
    //
    //     return await RequestAsync(request);
    // }
}