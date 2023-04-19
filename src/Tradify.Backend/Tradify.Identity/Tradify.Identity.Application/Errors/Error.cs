using System.Net;
using System.Text.Json.Serialization;
using Tradify.Identity.Application.Errors.Common;

namespace Tradify.Identity.Application.Errors;

public abstract class Error
{
    [JsonIgnore] public HttpStatusCode StatusCode { get; init; }
    public string Message { get; set; }
    
    public ErrorCode ErrorCode { get; set; }

    public Error(HttpStatusCode statusCode, string message, ErrorCode errorCode) =>
        (StatusCode, Message, ErrorCode) = (statusCode, message, errorCode);
}