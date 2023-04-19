using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.RestAPI.Results;

namespace Tradify.Identity.RestAPI.Controllers;

public class ApiControllerBase : ControllerBase
{
    private ISender _mediator;
    private IMapper _mapper;
    
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper =>
        _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected internal async Task<ApiResult<TValue>> RequestAsync<TValue>(
        IRequest<Result<TValue>> request, CancellationToken cancellationToken)
    {
        // non generic / generic
        var result = await _mediator.Send(request, cancellationToken);

        return Mapper.Map<ApiResult<TValue>>(result); // apiResult;
    }
    
}