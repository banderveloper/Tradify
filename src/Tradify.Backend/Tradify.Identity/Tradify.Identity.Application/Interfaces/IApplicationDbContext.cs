using Tradify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tradify.Identity.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<RefreshSession> RefreshSessions { get; set; }
        DbSet<UserData> UserDatas { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
