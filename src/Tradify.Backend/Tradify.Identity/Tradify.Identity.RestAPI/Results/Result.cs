using System.Net;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Errors;

namespace Tradify.Identity.RestAPI.Results;

public class Result : ActionResult
{
    public object? Data { get; set; }
    public Error? Error { get; set; }
    
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = Error == null ? (int)HttpStatusCode.OK : (int)Error.StatusCode;

        await response.WriteAsJsonAsync(this);
    }
}