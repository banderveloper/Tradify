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

public class RefreshCommand : IRequest<MediatorResult<Empty>> {}

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, MediatorResult<Empty>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public RefreshCommandHandler(
        IApplicationDbContext dbContext,
        JwtProvider jwtProvider,
        HttpContext context,
        CookieProvider cookieProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
        _context = context;
        _cookieProvider = cookieProvider;
    }
    
    public async Task<MediatorResult<Empty>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var result = new MediatorResult<Empty>();  
        
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken.Error is not null)
        {
            result.Error = refreshToken.Error;
            return result;
        }
        
        var existingSession = await _dbContext.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken.Data, cancellationToken);
        if (existingSession is null)
        {
            result.Error = new ExpectedError("Refresh session was not found", ErrorCode.RefreshSessionNotFound);
            return result;
        }

        existingSession.RefreshToken = Guid.NewGuid();
        existingSession.UpdatedAt = DateTime.Now;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == existingSession.UserId);
        if (user is null)
        {
            result.Error = new ExpectedError("User with found refresh session was not found",
                ErrorCode.UserByRefreshSessionNotFound);
            return result;
        }
        
        var jwtToken = _jwtProvider.CreateToken(user);
        
        _cookieProvider.AddJwtCookieToResponse(_context.Response,
            jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response, 
            existingSession.RefreshToken.ToString());

        return result;
    }
}