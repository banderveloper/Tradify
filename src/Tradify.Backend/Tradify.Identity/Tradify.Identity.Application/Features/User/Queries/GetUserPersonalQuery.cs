using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application.Features.User.Queries;

public class UserPersonalResponseModel
{
    public string UserName { get; set; }
    
    public string Email { get; set; }

    public string AvatarPath { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }

    public string? Phone { get; set; }
    
    public string? HomeAdress { get; set; }
    
    public DateOnly BirthDate { get; set; }
}

public class GetUserPersonalQuery : IRequest<Result<UserPersonalResponseModel?>>
{
    public int UserId { get; set; }
}

public class GetUserPersonalQueryHandler : IRequestHandler<GetUserPersonalQuery, Result<UserPersonalResponseModel?>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserPersonalQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<UserPersonalResponseModel?>> Handle(GetUserPersonalQuery request, CancellationToken cancellationToken)
    {
        var userPersonalResponseModel = await _dbContext.Users
            .Where(u => u.Id == request.UserId)
            .Include(u => u.UserData)
            .Select(u =>
                new UserPersonalResponseModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,

                    AvatarPath = u.UserData.AvatarPath,

                    FirstName = u.UserData.FirstName,
                    LastName = u.UserData.FirstName,
                    MiddleName = u.UserData.MiddleName,

                    Phone = u.UserData.Phone,

                    HomeAdress = u.UserData.HomeAddress,

                    BirthDate = u.UserData.BirthDate
                })
            .FirstOrDefaultAsync(cancellationToken);

        return userPersonalResponseModel;
    }
}