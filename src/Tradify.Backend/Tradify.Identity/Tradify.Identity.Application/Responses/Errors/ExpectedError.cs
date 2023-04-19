using System.Net;
using Tradify.Identity.Application.Responses.Errors.Common;

namespace Tradify.Identity.Application.Responses.Errors;

public sealed class ExpectedError : Error
{
    public ExpectedError(string message, ErrorCode errorCode) 
        : base(HttpStatusCode.OK, message,errorCode) {}
}