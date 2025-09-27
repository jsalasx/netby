using MsAuth.Domain.Entities;
using Shared.Dto;

namespace MsAuth.Domain.Ports;

public interface IUserRepository
{
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct);
    Task CreateAsync(UserEntity user, CancellationToken ct);
}
