namespace Shared.Infrastructure;
public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string RefreshSecret { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; } = 60;
}