namespace Tradify.Chat.Domain.Entities;

public class Message : BaseEntity
{
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    
    public string Body { get; set; }
    
    public Chat Chat { get; set; }
}