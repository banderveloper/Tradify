using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Tradify.Identity.Application.Common.MediatorResults;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Services;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public class RefreshCommand : IRequest<OneOf<Success,InvalidRefreshToken,UserNotFound>> {}

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, OneOf<Success,InvalidRefreshToken,UserNotFound>>
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
    
    public async Task<OneOf<Success,InvalidRefreshToken,UserNotFound>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken is null)
        {
            return new InvalidRefreshToken("Unable to extract refresh token from cookies.");
        }
        
        var existingSession = await _dbContext.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken, cancellationToken);
        if (existingSession is null)
        {
            return new InvalidRefreshToken("Session was not found.");
        }

        existingSession.RefreshToken = Guid.NewGuid();
        existingSession.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == existingSession.UserId, cancellationToken);
        if (user is null)
        {
            return new UserNotFound("User with sessions' user id was not found.");
        }
        
        var jwtToken = _jwtProvider.CreateToken(user);
        
        _cookieProvider.AddJwtCookieToResponse(_context.Response,
            jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response, 
            existingSession.RefreshToken.ToString());

        return new Success();
    }
}