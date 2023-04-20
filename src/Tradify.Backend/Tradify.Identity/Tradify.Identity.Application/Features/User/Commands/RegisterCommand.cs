using BCrypt.Net;
using MediatR;
using Tradify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;
using Tradify.Identity.Application.Responses.Errors.Common;
using Tradify.Identity.Application.Responses.Types;

namespace Tradify.Identity.Application.Features.User.Commands;

public class RegisterCommand : IRequest<MediatorResult<Empty>>
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

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, MediatorResult<Empty>>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MediatorResult<Empty>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = new MediatorResult<Empty>();
        
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u=>u.UserName == request.UserName || u.Email == request.Email,
                cancellationToken);

        if (existingUser is not null)
        {
            if (existingUser.UserName == request.UserName)
            {
                result.Error = new ExpectedError("User with given user name already exists",
                    ErrorCode.UserNameAlreadyExists);
                return result;
            }
            else // (user.Email == request.Email)
            {
                result.Error = new ExpectedError("User with given email already exists",
                    ErrorCode.EmailAlreadyExists);
                return result;
            }
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

        return result;
    }
}

