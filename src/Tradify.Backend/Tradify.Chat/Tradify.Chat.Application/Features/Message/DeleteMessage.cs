using MediatR;
using Tradify.Chat.Application.Errors;
using Tradify.Chat.Application.Errors.Common;
using Tradify.Chat.Application.Interfaces;
using Tradify.Chat.Application.Responses;

namespace Tradify.Chat.Application.Features.Message;

// Request
public class DeleteMessageCommand : IRequest<MediatorResult<bool>>
{
    public long MessageId { get; set; }
    public long SenderId { get; set; }
}

// Handler 
public class DeleteMessageCommandHandler :
    IRequestHandler<DeleteMessageCommand, MediatorResult<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteMessageCommandHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<MediatorResult<bool>> Handle(DeleteMessageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new MediatorResult<bool>();
        
        var messageToDelete = await _context.Messages
            .FindAsync(new object[] { request.MessageId }, cancellationToken);

        // Check message existing
        if (messageToDelete is null)
        {
            result.Error = new ExpectedError(errorCode: ErrorCode.MessageNotFound, message: "Message not found");
            return result;
        }

        // Check sender id and forbid if request user is not sender of message
        if (messageToDelete.SenderId != request.SenderId)
        {
            result.Error = new ExpectedError(errorCode: ErrorCode.MessageDeletionNotAllowed,
                message: "You are not the sender of current message, deletion is not allowed");
            return result;
        }

        // Delete message
        _context.Messages.Remove(messageToDelete);
        await _context.SaveChangesAsync(cancellationToken);

        result.Data = true;
        return result;
    }
}