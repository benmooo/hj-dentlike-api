using Dentlike.Application.DTOs;
using Dentlike.Application.Interfaces;
using Dentlike.Domain.Interfaces;

namespace Dentlike.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };
        }
    }
}
