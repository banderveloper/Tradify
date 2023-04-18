using System.Net;
using Tradify.Identity.Application.Errors.Common;

namespace Tradify.Identity.Application.Errors;

public abstract class Error
{
    public HttpStatusCode StatusCode { get; init; }
    public string Message { get; set; }
    
    public ErrorCode ErrorCode { get; set; }

    public Error(HttpStatusCode statusCode, string message, ErrorCode errorCode) =>
        (StatusCode, Message, ErrorCode) = (statusCode, message, errorCode);
}