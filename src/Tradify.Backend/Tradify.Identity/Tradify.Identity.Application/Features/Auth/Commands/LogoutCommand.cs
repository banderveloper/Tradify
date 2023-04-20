using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;
using Tradify.Identity.Application.Responses.Errors.Common;
using Tradify.Identity.Application.Responses.Types;
using Tradify.Identity.Application.Services;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<MediatorResult<Empty>> {}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, MediatorResult<Empty>>
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
    
    public async Task<MediatorResult<Empty>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var result = new MediatorResult<Empty>();
        
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken.Error is not null)
        {
            result.Error = refreshToken.Error;
            return result;
        }

        var existingSession = await _dbContext.RefreshSessions
            .FirstOrDefaultAsync(cancellationToken);
        if (existingSession is null)
        {
            result.Error = new ExpectedError("Refresh session by refresh token was not found",
                ErrorCode.RefreshSessionNotFound);
            return result;
        }

        //delete refresh from database
        _dbContext.RefreshSessions.Remove(existingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //delete from cookies
        _cookieProvider.DeleteJwtTokenFromCookies(_context.Response);
        _cookieProvider.DeleteRefreshTokenFromCookies(_context.Response);
        
        return result;
    }
}
