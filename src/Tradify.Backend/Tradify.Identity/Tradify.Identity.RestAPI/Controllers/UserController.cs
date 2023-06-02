using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Features.User.Commands;
using Tradify.Identity.Application.Features.User.Queries;
using Tradify.Identity.RestAPI.Models;

namespace Tradify.Identity.RestAPI.Controllers;

[Route("user")]
public class UserController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestModel requestModel)
    {
        var request = Mapper.Map<RegisterCommand>(requestModel);

        return await RequestAsync(request);
    }

    [HttpGet("public/{usersIds}")]
    public async Task<ActionResult> GetUsersByIds([FromRoute] IEnumerable<int> usersIds)
    {
        var request = new GetUsersQuery()
        {
            UsersIds = usersIds
        };

        return await RequestAsync(request);
    }
    
    [HttpGet("public/{userId:int}")]
    public async Task<ActionResult> GetUserProfileById([FromRoute] int userId)
    {
        var request = new GetUserProfileQuery()
        {
            UserId = userId
        };

        return await RequestAsync(request);
    }
    
    [Authorize]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult> GetUserPersonalById([FromRoute] int userId)
    {
        var request = new GetUserPersonalQuery()
        {
            UserId = userId
        };

        return await RequestAsync(request);
    }
}