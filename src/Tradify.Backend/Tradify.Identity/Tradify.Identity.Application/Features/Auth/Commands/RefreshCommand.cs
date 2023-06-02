using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Services;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public class RefreshCommand : IRequest<Result<Unit>> {}

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public RefreshCommandHandler(
        IApplicationDbContext dbContext,
        JwtProvider jwtProvider,
        IHttpContextAccessor accessor,
        CookieProvider cookieProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
        _context = accessor.HttpContext!;
        _cookieProvider = cookieProvider;
    }
    
    public async Task<Result<Unit>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken is null)
        {
            var message = "Error on getting refresh token from cookie.";
            var error = new ValidationException(new[]
            {
                new ValidationFailure(nameof(refreshToken),message)
            });
            return new Result<Unit>(error);
        }
        
        var existingSession = await _dbContext.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken, cancellationToken);
        if (existingSession is null)
        {
            var message = "Refresh session was not found.";
            var error = new ValidationException(new[]
            {
                new ValidationFailure(nameof(refreshToken),message)
            });
            return new Result<Unit>(error);
        }

        existingSession.RefreshToken = Guid.NewGuid();
        existingSession.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == existingSession.UserId, cancellationToken);
        if (user is null)
        {
            var message = "User with found refresh session was not found.";
            var error = new ValidationException(new[]
            {
                new ValidationFailure(nameof(refreshToken),message)
            });
            return new Result<Unit>(error);
        }
        
        var jwtToken = _jwtProvider.CreateToken(user);
        
        _cookieProvider.AddJwtCookieToResponse(_context.Response,
            jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response, 
            existingSession.RefreshToken.ToString());

        return Unit.Default;
    }
}