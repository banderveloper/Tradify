using BCrypt.Net;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Services;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Application.Features.Auth.Commands;


public class LoginCommand : IRequest<OneOf<Unit, ValidationException>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, OneOf<Unit, ValidationException>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public LoginCommandHandler(
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
    
    public async Task<OneOf<Unit,ValidationException>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);
        if (user is null)
        {
            var message = "User with given username does not exist.";
            var error = new ValidationException(new[]
            {
                new ValidationFailure(nameof(request.UserName),message)
            });
            return error;
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash, HashType.SHA512))
        {
            var message = "Invalid password.";
            var error = new ValidationException(new[]
            {
                new ValidationFailure(nameof(request.Password),message)
            });
            return error;
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
            _dbContext.RefreshSessions.Add(existingSession);
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

        return Unit.Value;
    }
}


