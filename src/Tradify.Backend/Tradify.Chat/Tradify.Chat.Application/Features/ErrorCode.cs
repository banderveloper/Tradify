using System.Text.Json.Serialization;
using Tradify.Chat.Application.Converters;

namespace Tradify.Chat.Application.Features;

[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
    UserNotInChat,
}