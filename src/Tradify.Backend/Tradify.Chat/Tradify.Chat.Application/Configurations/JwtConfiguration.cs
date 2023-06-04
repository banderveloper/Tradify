﻿namespace Tradify.Chat.Application.Configurations;

public class JwtConfiguration
{
    public static readonly string SectionName = "Jwt";
    
    public string JwtCookieName { get; set; }
    
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}