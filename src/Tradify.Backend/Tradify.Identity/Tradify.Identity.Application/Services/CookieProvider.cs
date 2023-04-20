using Microsoft.AspNetCore.Http;
using Tradify.Identity.Application.Common.Configurations;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;
using Tradify.Identity.Application.Responses.Errors.Common;

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
                Expires = new DateTimeOffset(DateTime.Now.AddHours(_refreshSessionConfiguration.HoursToExpiration))
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
    public MediatorResult<Guid> GetRefreshTokenFromCookie(HttpRequest request)
    {
        var result = new MediatorResult<Guid>();
        
        // Try to extract refresh token from cookie. If it is absent - exception
        var refreshTokenString = request.Cookies[_refreshSessionConfiguration.RefreshCookieName];
        if(refreshTokenString is null)
        {
            result.Error = new ExpectedError("Cookies do not contain refresh token",
                ErrorCode.RefreshInCookiesNotFound);
            return result;
        }
        
        if (!Guid.TryParse(refreshTokenString, out var refreshToken))
        {
            result.Error = new ExpectedError("Error on parsing refresh token from cookies",
                ErrorCode.RefreshParseError);
            return result;
        }

        result.Data = refreshToken;
        return result;
    }

    public void DeleteJwtTokenFromCookies(HttpResponse response)
    {
        response.Cookies.Delete(_jwtConfiguration.JwtCookieName);
    }

    public void DeleteRefreshTokenFromCookies(HttpResponse response)
    {
        response.Cookies.Delete(_refreshSessionConfiguration.RefreshCookieName);
    }
}
