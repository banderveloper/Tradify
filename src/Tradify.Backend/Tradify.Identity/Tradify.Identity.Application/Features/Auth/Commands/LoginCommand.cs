using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;
using Tradify.Identity.Application.Responses.Errors.Common;
using Tradify.Identity.Application.Services;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public record LoginCommand(string UserName,string Password) : IRequest<Result<Unit>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpResponse _response;
    private readonly CookieProvider _cookieProvider;

    public LoginCommandHandler(
        IApplicationDbContext context,
        JwtProvider jwtProvider,
        HttpResponse response,
        CookieProvider cookieProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
        _response = response;
        _cookieProvider = cookieProvider;
    }
    
    public async Task<Result<Unit>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new Result<Unit>();
        
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

        if (user is null)
        {
            result.Error = new ExpectedError("User with given user name does not exist", ErrorCode.UserNotFound);
            return result;
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
        {
            result.Error = new ExpectedError("Incorrect password", ErrorCode.UserNotFound);
            return result;
        }

        var jwtToken = _jwtProvider.CreateToken(user);
        var refreshToken = Guid.NewGuid();
        
        //finding existing session by user id
        var existingSession = await _context.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(
                session => session.UserId == user.Id,
                cancellationToken);
        
        //if session does not exist - create new session
        if (existingSession is null)
        {
            existingSession = new RefreshSession()
            {
                UserId = user.Id,
                RefreshToken = refreshToken
            };
        }
        //if session exists - update existing one
        else
        {
            existingSession.RefreshToken = refreshToken;
            existingSession.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        
        _cookieProvider.AddJwtCookieToResponse(_response, jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_response,Guid.NewGuid().ToString());

        return result;
    }
}


