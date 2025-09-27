using Microsoft.EntityFrameworkCore;
using MsAuth.Domain.Entities;
using MsAuth.Domain.Ports;

namespace MsAuth.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task CreateAsync(UserEntity user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }   
    

}
