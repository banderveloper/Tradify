using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Features.Auth.Commands;
using Tradify.Identity.RestAPI.Models;

namespace Tradify.Identity.RestAPI.Controllers;


[Route("auth")]
public class AuthController : ApiControllerBase
{
    [HttpGet("login")]
    public async Task<ActionResult> Login([FromQuery] LoginRequestModel requestModel)
    {
        //TODO: validation
        var request = Mapper.Map<LoginCommand>(requestModel);
        
        return await RequestAsync(request);
    }

    [HttpPut("refresh")]
    public async Task<ActionResult> Refresh()
    {
        var request = new RefreshCommand();
        
        return await RequestAsync(request);
    }

    [HttpDelete("logout")]
    public async Task<ActionResult> Logout()
    {
        var request = new LogoutCommand();

        return await RequestAsync(request);
    }
}