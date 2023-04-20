using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Features.User.Commands;
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
}