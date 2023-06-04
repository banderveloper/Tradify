using System.Net;

namespace Tradify.Chat.Application.Features;

public record struct PermissionError(string Message, ErrorCode ErrorCode);

public record struct NotFound(string Message, ErrorCode ErrorCode);