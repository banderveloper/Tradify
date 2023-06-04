using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tradify.Identity.RestAPI.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private IMediator _mediator;
    private IMapper _mapper;
    
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper =>
        _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
}