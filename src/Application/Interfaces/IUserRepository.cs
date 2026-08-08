using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByRefreshTokenAsync(string refreshToken);

        Task AddAsync(User user);

        Task SaveChangesAsync();
    }
}