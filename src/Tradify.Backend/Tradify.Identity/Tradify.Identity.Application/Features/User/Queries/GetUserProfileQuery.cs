using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application.Features.User.Queries;

public class UserProfileResponseModel
{
    public string AvatarPath { get; set; }
    
    public string UserName { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    
    public string? Phone { get; set; }
}

public class GetUserProfileQuery : IRequest<Result<UserProfileResponseModel?>>
{
    public int UserId { get; set; }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileResponseModel?>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserProfileQueryHandler(IApplicationDbContext dbContext) =>
        _dbContext = dbContext;

        public async Task<Result<UserProfileResponseModel?>> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userProfileResponseModel = await _dbContext.Users
            .Where(u => u.Id == request.UserId)
            .Include(u => u.UserData)
            .Select(u =>
                new UserProfileResponseModel()
                {
                    UserName = u.UserName,

                    AvatarPath = u.UserData.AvatarPath,
                    
                    FirstName = u.UserData.FirstName,
                    LastName = u.UserData.LastName,
                    MiddleName = u.UserData.MiddleName,

                    Phone = u.UserData.Phone,
                })
            .FirstOrDefaultAsync(cancellationToken);

        return userProfileResponseModel;
    }
}