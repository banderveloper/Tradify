namespace Tradify.Chat.Domain.Entities;

public class ChatUser
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
    
    public Chat Chat { get; set; }
}