using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;
using Tradify.Identity.Application.Responses.Errors.Common;
using Tradify.Identity.Application.Responses.Types;
using Tradify.Identity.Application.Services;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<MediatorResult<Empty>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, MediatorResult<Empty>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public LoginCommandHandler(
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
    
    public async Task<MediatorResult<Empty>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new MediatorResult<Empty>();
        
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);
        if (user is null)
        {
            result.Error = new ExpectedError("User with given user name does not exist", ErrorCode.UserNotFound);
            return result;
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash))
        {
            result.Error = new ExpectedError("Incorrect password", ErrorCode.PasswordInvalid);
            return result;
        }

        //finding existing session by user id
        var existingSession = await _dbContext.RefreshSessions
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
                RefreshToken = Guid.NewGuid()
            };
        }
        //if session exists - update existing one
        else
        {
            existingSession.RefreshToken = Guid.NewGuid();
            existingSession.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var jwtToken = _jwtProvider.CreateToken(user);
        
        _cookieProvider.AddJwtCookieToResponse(_context.Response, jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response,existingSession.RefreshToken.ToString());
        
        return result;
    }
}


