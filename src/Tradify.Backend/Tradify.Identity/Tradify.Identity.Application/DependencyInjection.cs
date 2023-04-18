using Microsoft.Extensions.DependencyInjection;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => 
                config.RegisterServicesFromAssemblies(typeof(IApplicationDbContext).Assembly));


            return services;
        }
    }
}
