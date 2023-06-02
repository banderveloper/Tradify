using BCrypt.Net;
using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;
using MediatR;
using Tradify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;

using Unit = LanguageExt.Unit;

namespace Tradify.Identity.Application.Features.User.Commands;

public class RegisterCommand : IRequest<Result<Unit>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Phone { get; set; }
    public string? HomeAddress { get; set; }
    
    public DateOnly BirthDate { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        List<ValidationFailure>? failures = null;
        if (await _dbContext.Users.AnyAsync(u=>u.UserName == request.UserName, cancellationToken))
        {
            failures ??= new List<ValidationFailure>();
            
            var message = "User with given user name already exists.";
            failures.Add(
                new ValidationFailure(nameof(request.UserName),message));
        }
        
        if (await _dbContext.Users.AnyAsync(u=>u.Email == request.Email, cancellationToken))
        { 
            failures ??= new List<ValidationFailure>();
            
            var message = "User with given email already exists.";
            failures.Add(
                new ValidationFailure(nameof(request.Email),message));
        }

        if(failures is not null)
        {
            var error = new ValidationException(failures);
            return new Result<Unit>(error);
        }

        var user = new Domain.Entities.User()
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, HashType.SHA512),
            UserData = new UserData()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Phone = request.Phone,
                HomeAddress = request.HomeAddress,
                BirthDate = request.BirthDate
            }
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Default;
    }
}

