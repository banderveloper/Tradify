using Tradify.Chat.Domain.Enums;

namespace Tradify.Chat.Domain.Entities;

public class SupportChatInfo : BaseEntity
{
    public int ChatId { get; set; }
    
    public SupportChatState State { get; set; }
    public int SupportUserId { get; set; }
    
    public Chat Chat { get; set; }
}