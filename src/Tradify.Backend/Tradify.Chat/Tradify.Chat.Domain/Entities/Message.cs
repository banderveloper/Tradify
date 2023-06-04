namespace Tradify.Chat.Domain.Entities;

public class Message : BaseEntity
{
    public long ChatId { get; set; }
    public long SenderId { get; set; }
    
    public string Body { get; set; }
    
    public Chat Chat { get; set; }
}