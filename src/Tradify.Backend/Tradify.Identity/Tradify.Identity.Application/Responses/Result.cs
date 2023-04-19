using AutoMapper;
using Tradify.Identity.Application.Mappings;
using Tradify.Identity.Application.Responses.Errors;

namespace Tradify.Identity.Application.Responses;

public class Result<TValue>
{
    public TValue? Data { get; set; }
    public Error? Error { get; set; }

    public Result(TValue? data = default, Error? error = null) =>
        (Data, Error) = (data, error);
}