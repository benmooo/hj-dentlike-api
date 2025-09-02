using Dentlike.Domain.Entities;

namespace Dentlike.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
    }
}
