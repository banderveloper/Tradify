using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tradify.Chat.RestAPI.Controllers;

[ApiController]
// [Authorize]
public class ApiControllerBase : ControllerBase
{
    protected long UserId = 1; // temporary
    
    private ISender _mediator;
    private IMapper _mapper;

    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper =>
        _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
    
    
}