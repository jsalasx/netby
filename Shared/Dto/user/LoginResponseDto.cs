namespace Shared.Dto.User;
public class LoginResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public long ExpiresIn { get; set; }
}