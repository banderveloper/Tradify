using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Tradify.Chat.Application.Features.Message.Commands;
using Tradify.Chat.Application.Mappings;

namespace Tradify.Chat.RestAPI.Models.Message;

public class CreateMessageRequestModel : IMappable
{
    [Required] [Range(1, long.MaxValue)] public long ChatId { get; set; }

    [Required] [MinLength(1)] public string Body { get; set; }

    public void Mapping(Profile profile) =>
        profile.CreateMap<CreateMessageRequestModel, CreateMessageCommand>();
}