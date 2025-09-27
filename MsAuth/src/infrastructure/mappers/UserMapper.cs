using MsAuth.Domain.Entities;
using Shared.Dto;
using Shared.Dto.User;

namespace MsAuth.Infrastructure.Mappers;

public static class UserMapper
{
    public static UserEntity ToEntity(CreateUserRequestDto dto)
    {
        return new UserEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    
}
