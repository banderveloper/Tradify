using Microsoft.EntityFrameworkCore;
using Tradify.Chat.Domain.Entities;

namespace Tradify.Chat.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Chat> Chats { get; set; }
    DbSet<ChatUser> ChatUsers { get; set; }
    DbSet<Message> Messages { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}