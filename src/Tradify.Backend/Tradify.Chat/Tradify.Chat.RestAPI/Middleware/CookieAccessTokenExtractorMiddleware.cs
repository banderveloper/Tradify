using Tradify.Chat.Application.Configurations;

namespace Tradify.Chat.RestAPI.Middleware;

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
        
        await _next.Invoke(context);
    }
}

public static class CookieAccessTokenExtractorMiddlewareExtension
{
    public static IApplicationBuilder UseCookieAccessTokenExtractor(this IApplicationBuilder app) =>
        app.UseMiddleware<CookieAccessTokenExtractorMiddleware>();
}