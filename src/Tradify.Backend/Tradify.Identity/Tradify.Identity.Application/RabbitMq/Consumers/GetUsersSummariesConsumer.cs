using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Features.User.Queries;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application.RabbitMq.Consumers;

public class GetUsersSummariesConsumer : IConsumer<GetUsersSummariesQuery>
{
    private readonly IMediator _mediator;

    public GetUsersSummariesConsumer(IMediator mediator) =>
        (_mediator) = (mediator);
    
    public async Task Consume(ConsumeContext<GetUsersSummariesQuery> context)
    {
        var getUsersSummariesQuery = context.Message;

        var result = _mediator.Send(getUsersSummariesQuery);

        await context.RespondAsync(result);
    }
}