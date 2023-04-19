using System.Net;
using Tradify.Chat.Application.Errors.Common;

namespace Tradify.Chat.Application.Errors;

public sealed class ExpectedError : Error
{
    public ExpectedError(string message, ErrorCode errorCode)
        : base(HttpStatusCode.OK, message, errorCode) {}
}