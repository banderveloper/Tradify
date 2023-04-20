using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Domain.Entities;
using Tradify.Identity.Persistence.EntityTypeConfigurations;

namespace Tradify.Identity.Persistence
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        public override DbSet<User> Users { get; set; }
        public override DbSet<RefreshSession> RefreshSessions { get; set; }
        public override DbSet<UserData> UserDatas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RefreshSessionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
