using Microsoft.AspNetCore.Mvc;
using Tradify.Chat.Application.Features.Message.Commands;
using Tradify.Chat.RestAPI.Models.Message;

namespace Tradify.Chat.RestAPI.Controllers;

[Route("api/message")]
public class MessageController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequestModel model)
    {
        var command = Mapper.Map<CreateMessageCommand>(model);
        command.SenderId = UserId;

        var result = await Mediator.Send(command);

        return result.Match<IActionResult>(
            success => Ok(),
            permissionError => Ok(result.Value)
        );
    }
}