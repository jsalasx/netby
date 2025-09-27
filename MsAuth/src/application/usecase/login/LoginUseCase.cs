using MsAuth.Domain.Entities;
using MsAuth.Domain.Ports;
using Shared.Application.Service;
using Shared.Dto.User;
using Shared.Infrastructure;


namespace MsAuth.Application.UseCase;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public LoginUseCase(IUserRepository userRepository, JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings;
        
    }


    public async Task<LoginResponseDto> Execute(LoginRequestDto user, CancellationToken ct = default)
    {
        // Validaciones de negocio podrían ir aquí
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is required", nameof(user.Email));

        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Password is required", nameof(user.Password));

        var existingUser = await _userRepository.GetByEmailAsync(user.Email, ct);

        if (existingUser == null || existingUser.Password != user.Password)
            throw new UnauthorizedAccessException("Invalid email or password");

        var claims = new Dictionary<string, string>
        {
            { "email", existingUser.Email },
            { "id", existingUser.Id.ToString() }
        };

        var audience = "ms-products,ms-transactions";
        var issuer = "ms-auth";
        var token = JwtService.GenerateToken(_jwtSettings.Secret, issuer, audience, claims, _jwtSettings.ExpiresInMinutes);
        var refreshToken = JwtService.GenerateToken(_jwtSettings.RefreshSecret, issuer, audience, claims, _jwtSettings.ExpiresInMinutes * 24 * 7); // Refresh token válido por 7 días
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expiresIn = now + (_jwtSettings.ExpiresInMinutes * 60);
        return new LoginResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresIn = expiresIn
        };
    }
}