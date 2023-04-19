using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Options;

namespace Tradify.Identity.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            // Get service configuration from services
            var scope = services.BuildServiceProvider().CreateScope();
            var dbConfig = scope.ServiceProvider.GetRequiredService<DatabaseOptions>();

            // register db context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var filledConnectionString = string.Format(dbConfig.ConnectionString, dbConfig.User, dbConfig.Password);

                //Console.WriteLine("Filled conn string: " + filledConnectionString);

                //options.UseSqlite(connectionConfiguration!.Sqlite);
                options.UseNpgsql(filledConnectionString);

                // for optimizing read-only queries, disabling caching of entities
                // in ef select queries before update should be added .AsTracking method  
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // bind db context interface to class
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
