using System.Text.Json.Serialization;
using Tradify.Chat.Application.Converters;

namespace Tradify.Chat.Application.Errors.Common;

/// <summary>
/// Exception error code, sent to client with error model as snake_case_string
/// </summary>
/// <example>CookieRefreshTokenNotPassed => cookie_refresh_token_not_passed</example>
[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
    UserNotInChat,
    MessageNotFound,
    MessageDeletionNotAllowed
}