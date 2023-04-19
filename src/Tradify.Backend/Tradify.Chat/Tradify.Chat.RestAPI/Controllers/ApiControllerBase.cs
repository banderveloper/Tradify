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
    protected ISender Mediator =>
        HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected IMapper Mapper =>
        HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected internal async Task<ApiResult<TValue>> RequestAsync<TValue>(
        IRequest<Result<TValue>> request, CancellationToken cancellationToken)
    {
        // non generic / generic
        var result = await Mediator.Send(request, cancellationToken);

        return Mapper.Map<ApiResult<TValue>>(result); // apiResult;
    }
}