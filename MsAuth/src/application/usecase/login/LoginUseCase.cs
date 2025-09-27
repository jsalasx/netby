using MsAuth.Domain.Entities;
using MsAuth.Domain.Ports;
using Shared.Application.Service;
using Shared.Dto.User;


namespace MsAuth.Application.UseCase;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly string _jwtSecret;
    private readonly string _jwtRefreshSecret;

    private readonly int _expiresInMinutes;

    public LoginUseCase(IUserRepository userRepository, string jwtSecret, string jwtRefreshSecret, int expiresInMinutes = 60)
    {
        _userRepository = userRepository;
        _jwtSecret = jwtSecret;
        _jwtRefreshSecret = jwtRefreshSecret;
        _expiresInMinutes = expiresInMinutes;
        
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
        var token = JwtService.GenerateToken(_jwtSecret, issuer, audience, claims, _expiresInMinutes);
        var refreshToken = JwtService.GenerateToken(_jwtRefreshSecret, issuer, audience, claims, _expiresInMinutes);
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expiresIn = now + (_expiresInMinutes * 60);
        return new LoginResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresIn = expiresIn
        };
    }
}