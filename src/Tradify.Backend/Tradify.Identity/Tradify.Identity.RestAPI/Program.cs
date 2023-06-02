using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Tradify.Identity.Application;
using Tradify.Identity.Application.Common.Converters;
using Tradify.Identity.Application.Common.Mappings;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Domain.Enums;
using Tradify.Identity.Persistence;
using Tradify.Identity.RestAPI;
using Tradify.Identity.RestApi.Middleware;

// Allow DateTime for postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Configuration.AddEnvironmentVariables();

//Configures JwtConfiguration, RefreshSessionConfiguration and DatabaseConfiguration
builder.Services.AddCustomConfigurations(builder.Configuration);

builder.Services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = 
            JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = 
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

//DI for layers
builder.Services.AddApplication();
builder.Services.AddPersistence();

builder.Services.AddCors(options =>
{
    // All clients (temporary)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<UserRole>());
        //options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<ErrorCode>());
    });

builder.Services.AddAutoMapper(options =>
{
    //applying all Auto Mapper configurations from Application Layer
    options.AddProfile(new AssemblyMappingProfile(typeof(IApplicationDbContext).Assembly));
    //applying all Auto Mapper configurations from Presentation Layer
    options.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
});

//swagger
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Cookie,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        DatabaseInitializer.Initialize(context);
    }
    catch(Exception ex)
    {
        //TODO: refactor this
        Console.WriteLine(ex.Message);
    }
}

app.UseCustomExceptionHandler();
app.UseCookieAccessTokenExtractor();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    // show swagger page using root Uri
    config.RoutePrefix = string.Empty;

    config.SwaggerEndpoint("swagger/v1/swagger.json", "Tradify Identity RestAPI");
});

app.MapControllers();

app.Run();