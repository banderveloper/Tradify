using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Features.Auth.Commands;
using Tradify.Identity.RestAPI.Models;

namespace Tradify.Identity.RestAPI.Controllers;


[Route("auth")]
public class AuthController : ApiControllerBase
{
    private IMapper _mapper;

    public AuthController(IMapper mapper) =>
        (_mapper) = (mapper);

    [HttpGet("login")]
    public async Task<ActionResult> Login(LoginRequestModel requestModel, CancellationToken cancellationToken)
    {
        //TODO: validation
        var request = _mapper.Map<LoginCommand>(requestModel);
        
        return await RequestAsync(request, cancellationToken);
    }
}