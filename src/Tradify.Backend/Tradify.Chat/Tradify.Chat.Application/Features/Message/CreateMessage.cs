using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tradify.Chat.Application.Errors;
using Tradify.Chat.Application.Errors.Common;
using Tradify.Chat.Application.Interfaces;
using Tradify.Chat.Application.Mappings;
using Tradify.Chat.Application.Responses;

namespace Tradify.Chat.Application.Features.Message;

// Request
public class CreateMessageCommand : IRequest<MediatorResult<CreateMessageResponseModel>>
{
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string Body { get; set; }
}

// Response
public class CreateMessageResponseModel : IMappable
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string Body { get; set; }

    public void Mapping(Profile profile) =>
        profile.CreateMap<Domain.Entities.Message, CreateMessageResponseModel>();
}

// Handler
public class CreateMessageCommandHandler :
    IRequestHandler<CreateMessageCommand, MediatorResult<CreateMessageResponseModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateMessageCommandHandler(IApplicationDbContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);

    
    public async Task<MediatorResult<CreateMessageResponseModel>> Handle(CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new MediatorResult<CreateMessageResponseModel>();

        // Check if user in chat before sending message

        var chatUserConnection = await _context.ChatUsers
            .FirstOrDefaultAsync(cu => cu.UserId == request.SenderId &&
                                       cu.ChatId == request.ChatId, cancellationToken);

        if (chatUserConnection is null)
        {
            result.Error = new ExpectedError(errorCode: ErrorCode.UserNotInChat,
                message: "User doesn't exists in current chat");
            return result;
        }

        // Create new message
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

        // Convert message to response model
        result.Data = _mapper.Map<CreateMessageResponseModel>(newMessage);

        return result;
    }
}