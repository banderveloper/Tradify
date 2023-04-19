namespace Tradify.Identity.Application.Configurations;

public class RefreshSessionConfiguration
{
    public static readonly string RefreshSessionSection = "RefreshSession";
    
    public string RefreshCookieName { get; set; }
    public int RefreshCookieLifetimeHours { get; set; }
}