using Tradify.Chat.Application.Errors;

namespace Tradify.Chat.Application.Responses;

public class MediatorResult<TValue>
{
    public TValue? Data { get; set; }
    public Error? Error { get; set; }
    
    public MediatorResult(TValue? data = default, Error? error = null) =>
        (Data, Error) = (data, error);
}