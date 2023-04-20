using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Tradify.Chat.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => 
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        
        

        return services;
    }
}