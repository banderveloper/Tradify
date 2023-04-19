using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.RestAPI.Models;

namespace Tradify.Identity.RestAPI.Controllers;


[Route("auth")]
public class AuthController : ApiControllerBase
{
    [HttpGet("login")]
    public async Task<ActionResult> Login(LoginRequestModel requestModel, CancellationToken cancellationToken)
    {
        //TODO: validation


        var result = await Mediator.Send(requestModel, cancellationToken);

        return result;
    }
}