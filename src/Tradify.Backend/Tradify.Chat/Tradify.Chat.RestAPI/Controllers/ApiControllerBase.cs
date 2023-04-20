using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tradify.Chat.Application.Responses;
using Tradify.Chat.RestAPI.Results;

namespace Tradify.Chat.RestAPI.Controllers;

[ApiController]
// [Authorize]
public class ApiControllerBase : ControllerBase
{
    private IMediator _mediator;
    private IMapper _mapper;

    protected ISender Mediator =>
        HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected IMapper Mapper =>
        _mapper = HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected async Task<ApiResult<TValue>> RequestAsync<TValue>(
        IRequest<MediatorResult<TValue>> request)
    {
        var mediatorResult = await Mediator.Send(request);
        return new ApiResult<TValue>(mediatorResult);
    }
}