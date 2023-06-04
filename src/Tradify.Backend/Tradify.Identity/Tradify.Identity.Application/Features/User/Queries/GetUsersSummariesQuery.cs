using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Tradify.Identity.Application.Common.MediatorResults;
using Tradify.Identity.Application.Interfaces;
using NotFound = Tradify.Identity.Application.Common.MediatorResults.NotFound;

namespace Tradify.Identity.Application.Features.User.Queries;

public class UserSummaryResponseModel
{
    public long Id { get; set; }
    public string AvatarPath { get; set; }
    public string UserName { get; set; }
}

public class GetUsersSummariesQuery : IRequest<OneOf<Success<IEnumerable<UserSummaryResponseModel>>,NotFound>>
{
    public IEnumerable<long> UsersIds { get; set; }
}

public class GetUsersSummariesQueryHandler : IRequestHandler<GetUsersSummariesQuery, OneOf<Success<IEnumerable<UserSummaryResponseModel>>,NotFound>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUsersSummariesQueryHandler(IApplicationDbContext dbContext) =>
        (_dbContext) = (dbContext);
        
    public async Task<OneOf<
            Success<IEnumerable<UserSummaryResponseModel>>, 
            NotFound>> 
        Handle(GetUsersSummariesQuery request, CancellationToken cancellationToken)
    {
        var usersSummariesResponseModels = await _dbContext
            .Users
            .Where(user => request.UsersIds.Contains(user.Id))
            .Include(user => user.UserData)
            .Select(user =>
                new UserSummaryResponseModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    AvatarPath = user.UserData.AvatarPath
                })
            .ToListAsync(cancellationToken);

        if (usersSummariesResponseModels is null)
            return new NotFound("Users with given ids were not found", ErrorCode.UserNotFound);

        return new Success<IEnumerable<UserSummaryResponseModel>>(usersSummariesResponseModels);
    }
}