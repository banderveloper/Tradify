using System.Net;
using Tradify.Identity.Application.Responses.Errors.Common;

namespace Tradify.Identity.Application.Responses.Errors;

public sealed class NotAcceptableError : Error
{
    public NotAcceptableError(string message, ErrorCode errorCode)
        : base(HttpStatusCode.NotAcceptable,message, errorCode) {}
}