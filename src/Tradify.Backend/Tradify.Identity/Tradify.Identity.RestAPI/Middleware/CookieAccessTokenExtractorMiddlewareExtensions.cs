namespace Tradify.Identity.RestApi.Middleware;

public static class CookieAccessTokenExtractorMiddlewareExtensions
{
    public static IApplicationBuilder UseCookieAccessTokenExtractor(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CookieAccessTokenExtractorMiddleware>();
    }
}