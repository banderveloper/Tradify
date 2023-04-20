using Tradify.Chat.Domain.Enums;

namespace Tradify.Chat.Domain.Entities;

public class SupportChatInfo : BaseEntity
{
    public long ChatId { get; set; }
    public long SupportUserId { get; set; }
    
    public SupportChatState State { get; set; }
    
    public Chat Chat { get; set; }
}