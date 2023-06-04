using System.Net;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Services;
using OneOf;
using OneOf.Types;
using Tradify.Identity.Application.Common.MediatorResults;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<OneOf<Success,InvalidData>> {}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, OneOf<Success,InvalidData>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;
    
    public LogoutCommandHandler(
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
    
    public async Task<OneOf<Success,InvalidData>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken is null)
        {
            return new InvalidData("Unable to extract refresh token from cookies.", ErrorCode.InvalidRefreshToken);
        }

        var existingSession = await _dbContext.RefreshSessions
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken, cancellationToken);
        if (existingSession is null)
        {
            return new InvalidData("Session was not found.", ErrorCode.InvalidRefreshToken);
        }

        //delete refresh from database
        _dbContext.RefreshSessions.Remove(existingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //delete from cookies
        _cookieProvider.DeleteJwtTokenFromCookies(_context.Response);
        _cookieProvider.DeleteRefreshTokenFromCookies(_context.Response);
        
        return new Success();
    }
}
