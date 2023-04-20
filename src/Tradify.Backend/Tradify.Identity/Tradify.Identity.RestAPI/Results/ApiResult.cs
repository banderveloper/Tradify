using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Common.Mappings;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;

namespace Tradify.Identity.RestAPI.Results;

public class ApiResult<TEntity> : ActionResult
{
    public TEntity? Data { get; set; }
    public Error? Error { get; set; }

    public ApiResult(MediatorResult<TEntity> mediatorResult)
        => (Data, Error) = (mediatorResult.Data, mediatorResult.Error);
    
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = Error == null ? (int)HttpStatusCode.OK : (int)Error.StatusCode;

        await response.WriteAsJsonAsync(this);
    }
}