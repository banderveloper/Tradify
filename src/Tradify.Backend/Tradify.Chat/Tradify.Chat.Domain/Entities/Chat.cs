namespace Tradify.Chat.Domain.Entities;

public class Chat : BaseEntity
{
    public string Title { get; set; }
    
    public IEnumerable<Message> Messages { get; set; }
}