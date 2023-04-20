using Microsoft.AspNetCore.Mvc;
using Tradify.Chat.Application.Features.Message;
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

        return await RequestAsync(command);
    }

    [HttpDelete("{messageId:long}")]
    public async Task<IActionResult> DeleteMessage(long messageId)
    {
        var command = new DeleteMessageCommand
        {
            MessageId = messageId,
            SenderId = UserId
        };

        return await RequestAsync(command);
    } 
}