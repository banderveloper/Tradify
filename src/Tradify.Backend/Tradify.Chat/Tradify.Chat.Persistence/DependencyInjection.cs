using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tradify.Chat.Application.Configurations;
using Tradify.Chat.Application.Interfaces;

namespace Tradify.Chat.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var scope = services.BuildServiceProvider().CreateScope();
        var dbConfig = scope.ServiceProvider.GetRequiredService<DatabaseConfiguration>();

        // register db context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var filledConnectionString = string.Format(dbConfig.ConnectionString, dbConfig.User, dbConfig.Password);
            
            options.UseNpgsql(filledConnectionString);
            
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        // bind db context interface to class
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}