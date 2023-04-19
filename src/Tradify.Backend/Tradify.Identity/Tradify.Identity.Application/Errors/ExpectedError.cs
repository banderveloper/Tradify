using System.Net;
using Tradify.Identity.Application.Errors.Common;

namespace Tradify.Identity.Application.Errors;

public sealed class ExpectedError : Error
{
    public ExpectedError(string message, ErrorCode errorCode) 
        : base(HttpStatusCode.OK, message,errorCode) {}
}