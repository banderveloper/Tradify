using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Services;

namespace Tradify.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<JwtProvider>();
            services.AddScoped<CookieProvider>();
            
            services.AddMediatR(config => 
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


            return services;
        }
    }
}
