using MsAuth.Domain.Entities;
using MsAuth.Domain.Ports;
using Shared.Application.Service;
using Shared.Dto.User;


namespace MsAuth.Application.UseCase;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;

    public RegisterUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
       
        
    }


    public async Task<CreateUserResponseDto> Execute(UserEntity user, CancellationToken ct = default)
    {
        // Validaciones de negocio podrían ir aquí
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is required", nameof(user.Email));

        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Password is required", nameof(user.Password));

        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("Name is required", nameof(user.Name));

        var existingUser = await _userRepository.GetByEmailAsync(user.Email, ct);

        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.CreateAsync(user, ct);
        return new CreateUserResponseDto
        {
            IsSuccess = true,
            Message = "User registered successfully",
            UserId = user.Id
        };
    }
}