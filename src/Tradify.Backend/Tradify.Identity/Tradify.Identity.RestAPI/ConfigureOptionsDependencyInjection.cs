using Tradify.Identity.Application.Configurations;

namespace Tradify.Identity.RestAPI;

public static class ConfigureOptionsDependencyInjection
{
    public static IServiceCollection AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(
            configuration.GetRequiredSection(JwtConfiguration.JwtSection));

        //TODO: configure jwt bearer using additional class
        //services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<RefreshSessionConfiguration>(
            configuration.GetRequiredSection(RefreshSessionConfiguration.RefreshSessionSection));

        return services;
    }
}