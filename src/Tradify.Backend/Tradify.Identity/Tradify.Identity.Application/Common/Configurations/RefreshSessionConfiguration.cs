namespace Tradify.Identity.Application.Common.Configurations;

public class RefreshSessionConfiguration
{
    public static readonly string RefreshSessionSection = "RefreshSession";
    
    public string RefreshCookieName { get; set; }
    public int HoursToExpiration { get; set; }
}