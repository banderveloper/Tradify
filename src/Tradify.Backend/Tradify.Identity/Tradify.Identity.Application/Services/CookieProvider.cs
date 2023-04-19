using Microsoft.AspNetCore.Http;
using Tradify.Identity.Application.Configurations;
using Tradify.Identity.Application.Responses;

namespace Tradify.Identity.Application.Services;

public class CookieProvider
{
    private readonly RefreshSessionConfiguration _refreshSessionConfiguration;
    private readonly JwtConfiguration _jwtConfiguration;
    
    public CookieProvider(
        RefreshSessionConfiguration refreshSessionConfiguration,
        JwtConfiguration jwtConfiguration) =>
        (_refreshSessionConfiguration,_jwtConfiguration) = (refreshSessionConfiguration,jwtConfiguration);
    
    // Insert refresh token into response http-only cookie
    public void AddRefreshCookieToResponse(HttpResponse response, string value)
    {
        response.Cookies.Append(_refreshSessionConfiguration.RefreshCookieName, value,
            new CookieOptions()
            {
                HttpOnly = true, 
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(DateTime.Now.AddHours(_refreshSessionConfiguration.RefreshCookieLifetimeHours))
            });
    }
    
    public void AddJwtCookieToResponse(HttpResponse response, string value)
    {
        response.Cookies.Append(_jwtConfiguration.JwtCookieName, value,
            new CookieOptions()
            {
                HttpOnly = true, 
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(DateTime.Now.AddMinutes(_jwtConfiguration.MinutesToExpiration))
            });
    }

    // Extract refresh token from http-only cookie
    public Guid GetRefreshTokenFromCookie(HttpRequest request)
    {
        if (!request.Cookies.ContainsKey(_refreshSessionConfiguration.RefreshCookieName))
            return new Result()
        
        // Try to extract refresh token from cookie. If it is absent - exception
        if (!request.Cookies.TryGetValue(_refreshSessionConfiguration.RefreshCookieName, out var refreshToken))
            throw new NotAcceptableRequestException { ErrorCode = ErrorCode.CookieRefreshTokenNotPassed };

        return Guid.Parse(refreshToken);
    }
}