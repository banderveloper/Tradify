using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Tradify.Identity.Application.Common.MediatorResults;

public enum ErrorCode
{
    InvalidCredentials,
    InvalidRefreshToken,
    UserAlreadyExists,
    UserNotFound,
}

public record struct InvalidData(string Message, ErrorCode ErrorCode);

public record struct NotFound(string Message, ErrorCode ErrorCode);

public record struct AlreadyExists(string Message, ErrorCode ErrorCode);