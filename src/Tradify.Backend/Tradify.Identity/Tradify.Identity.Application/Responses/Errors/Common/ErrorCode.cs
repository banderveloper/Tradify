using System.Text.Json.Serialization;
using Tradify.Identity.Application.Common.Converters;

namespace Tradify.Identity.Application.Responses.Errors.Common;

[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
        
    UserNotFound,
    PasswordInvalid,
        
    RefreshSessionNotFound,
    UserByRefreshSessionNotFound,
        
    RefreshInCookiesNotFound,
    RefreshParseError,
        
    UserNameAlreadyExists,
    EmailAlreadyExists,
}