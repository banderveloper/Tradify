using AutoMapper;
using Tradify.Identity.Application.Features.Auth.Commands;
using Tradify.Identity.Application.Common.Mappings;

namespace Tradify.Identity.RestAPI.Models;

public class LoginRequestModel : IMappable
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<LoginRequestModel, LoginCommand>();
    }
}