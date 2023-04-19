using Tradify.Chat.Application;
using Tradify.Chat.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication().AddPersistence();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();