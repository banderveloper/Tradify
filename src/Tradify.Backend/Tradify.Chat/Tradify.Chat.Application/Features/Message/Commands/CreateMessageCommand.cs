using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Tradify.Chat.Application.Interfaces;
using Tradify.Chat.Application.Mappings;

namespace Tradify.Chat.Application.Features.Message.Commands;

// Command
public class CreateMessageCommand : IRequest<OneOf<Success, PermissionError>>
{
    public long ChatId { get; set; }
    public long SenderId { get; set; }
    public string Body { get; set; }
}

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, OneOf<Success, PermissionError>>
{
    private readonly IApplicationDbContext _context;

    public CreateMessageCommandHandler(IApplicationDbContext context, IMapper mapper)
        => _context = context;

    public async Task<OneOf<Success, PermissionError>> Handle(CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        // Check if user in chat when he tries to create message
        var isUserInChat = await _context.ChatUsers
            .AnyAsync(cu => cu.UserId == request.SenderId &&
                            cu.ChatId == request.ChatId, cancellationToken);

        if (!isUserInChat)
            return new PermissionError(
                "User doesn't exists in current chat",
                ErrorCode.UserNotInChat);
        
        // If ok - create message in db
        var newMessage = new Domain.Entities.Message
        {
            ChatId = request.ChatId,
            SenderId = request.SenderId,
            Body = request.Body,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}