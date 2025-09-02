using Dentlike.Application.DTOs;

namespace Dentlike.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
    }
}
