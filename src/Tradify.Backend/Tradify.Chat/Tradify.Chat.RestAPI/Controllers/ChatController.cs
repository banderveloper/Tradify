using Microsoft.AspNetCore.Mvc;

namespace Tradify.Chat.RestAPI.Controllers;

[Microsoft.AspNetCore.Components.Route("api/chat")]
public class ChatController : ApiControllerBase
{
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        return Ok("Hello world");
    } 
}