using Tradify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tradify.Identity.Application.Interfaces
{
    public abstract class IApplicationDbContext : DbContext
    {
        public abstract DbSet<User> Users { get; set; }
        public abstract DbSet<RefreshSession> RefreshSessions { get; set; }
        public abstract DbSet<UserData> UserDatas { get; set; }

        protected IApplicationDbContext(DbContextOptions options):base(options){}
    }
}
