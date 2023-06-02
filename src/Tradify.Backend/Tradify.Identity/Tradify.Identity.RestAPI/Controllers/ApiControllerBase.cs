using AutoMapper;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Common.Extensions;

namespace Tradify.Identity.RestAPI.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender _mediator;
    private IMapper _mapper;
    
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper =>
        _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
}