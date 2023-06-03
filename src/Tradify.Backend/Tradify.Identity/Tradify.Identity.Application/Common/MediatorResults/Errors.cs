using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Tradify.Identity.Application.Common.MediatorResults;

public record struct InvalidCredentials(string Message);

public record struct InvalidRefreshToken(string Message);

public record struct UserNotFound(string Message);

public record struct UserAlreadyExists(string Message);
