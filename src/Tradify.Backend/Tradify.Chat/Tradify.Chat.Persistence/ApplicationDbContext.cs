using Microsoft.EntityFrameworkCore;
using Tradify.Chat.Application.Interfaces;
using Tradify.Chat.Domain.Entities;

namespace Tradify.Chat.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Domain.Entities.Chat> Chats { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<Message> Messages { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatUser>().HasNoKey();
        base.OnModelCreating(modelBuilder);
    }
}