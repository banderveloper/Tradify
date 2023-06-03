using BCrypt.Net;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Tradify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Tradify.Identity.Application.Common.MediatorResults;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application.Features.User.Commands;

public class RegisterCommand : IRequest<OneOf<Success, UserAlreadyExists, ValidationResult>>
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

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OneOf<Success, UserAlreadyExists, ValidationResult>>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OneOf<Success, UserAlreadyExists, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        List<ValidationFailure>? failures = null;
        if (await _dbContext.Users.AnyAsync(u=>u.UserName == request.UserName, cancellationToken))
        {
            return new UserAlreadyExists("User with given user name already exists.");
        }
        
        if (await _dbContext.Users.AnyAsync(u=>u.Email == request.Email, cancellationToken))
        {
            return new UserAlreadyExists("User with given email already exists.");
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

        return new Success();
    }
}

