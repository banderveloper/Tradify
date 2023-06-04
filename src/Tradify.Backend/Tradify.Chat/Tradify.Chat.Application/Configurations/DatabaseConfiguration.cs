namespace Tradify.Chat.Application.Configurations;

public class DatabaseConfiguration
{
    public static readonly string SectionName = "Database";
    
    public string ConnectionString { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}