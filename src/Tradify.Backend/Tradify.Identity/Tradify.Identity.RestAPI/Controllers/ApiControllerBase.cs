using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.RestAPI.Results;

namespace Tradify.Identity.RestAPI.Controllers;

public class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}