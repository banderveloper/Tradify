using System.Reflection;
using Microsoft.OpenApi.Models;
using Tradify.Identity.Application.Mappings;
using Tradify.Identity.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(options =>
{
    //applying all Auto Mapper configurations from Application Layer
    options.AddProfile(new AssemblyMappingProfile(typeof(IApplicationDbContext).Assembly));
    //applying all Auto Mapper configurations from Presentation Layer
    options.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
});

builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var app = builder.Build();


app.Run();