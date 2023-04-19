using System.Net;
using Tradify.Chat.Application.Errors.Common;

namespace Tradify.Chat.Application.Errors;

public sealed class NotAcceptableError : Error
{
    public NotAcceptableError(string message, ErrorCode errorCode) 
        : base(HttpStatusCode.NotAcceptable, message, errorCode) {}
}