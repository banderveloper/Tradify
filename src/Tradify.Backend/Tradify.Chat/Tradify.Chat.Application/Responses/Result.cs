using Tradify.Chat.Application.Errors;

namespace Tradify.Chat.Application.Responses;

public class Result<TValue>
{
    public TValue? Data { get; set; }
    public Error? Error { get; set; }
    
    public Result(TValue? data = default, Error? error = null) =>
        (Data, Error) = (data, error);
}