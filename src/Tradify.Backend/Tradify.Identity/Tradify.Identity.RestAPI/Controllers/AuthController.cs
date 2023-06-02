using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Common.Extensions;
using Tradify.Identity.Application.Features.Auth.Commands;
using Tradify.Identity.RestAPI.Models;

namespace Tradify.Identity.RestAPI.Controllers;


[Route("auth")]
public class AuthController : ApiControllerBase
{
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] LoginRequestModel requestModel)
    {
        //TODO: validation
        var request = Mapper.Map<LoginCommand>(requestModel);

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            output => Ok(),
            ex => NotFound(ex.ToProblemDetails()));
    }

    [HttpPut("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var request = new RefreshCommand();
        
        return 
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var request = new LogoutCommand();

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            output => Ok(),
            ex => NotFound(ex.ToProblemDetails()));
    }
}