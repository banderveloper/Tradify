using Microsoft.AspNetCore.Authentication.JwtBearer;
using Tradify.Identity.Application.Configurations;

namespace Tradify.Identity.RestApi.Middleware;

public class CookieAccessTokenExtractorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfiguration _configuration;

    public CookieAccessTokenExtractorMiddleware(RequestDelegate next, JwtConfiguration configuration) =>
        (_next, _configuration) = (next, configuration);

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies[_configuration.JwtCookieName];
        
        if (!string.IsNullOrEmpty(token))
            context.Request.Headers.Add("Authorization", $"Bearer {token}");
        
        await _next(context);
    }
}

public static class CookieAccessTokenExtractorMiddlewareExtensions
{
    public static IApplicationBuilder UseCookieAccessTokenExtractor(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CookieAccessTokenExtractorMiddleware>();
    }
}