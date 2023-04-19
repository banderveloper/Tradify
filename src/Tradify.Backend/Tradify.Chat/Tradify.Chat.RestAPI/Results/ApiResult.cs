using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tradify.Chat.Application.Errors;
using Tradify.Chat.Application.Mappings;
using Tradify.Chat.Application.Responses;

namespace Tradify.Chat.RestAPI.Results;

public class ApiResult<TEntity> : ActionResult, IMappable
{
    public TEntity? Data { get; set; }
    public Error? Error { get; set; }
    
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = Error == null ? (int)HttpStatusCode.OK : (int)Error.StatusCode;

        await response.WriteAsJsonAsync(this);
    }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Result<TEntity>, ApiResult<TEntity>>();
    }
}