namespace Tradify.Chat.Domain.Entities;

public class ChatUser : BaseEntity
{
    public long ChatId { get; set; }
    public long UserId { get; set; }
    
    public Chat Chat { get; set; }
}