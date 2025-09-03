using Dentlike.Application.DTOs;
using Dentlike.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentlike.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // all users
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllUsers()
        {
            _logger.LogInformation("Fetching all users");
            // This is a placeholder implementation. Replace with actual data retrieval logic.
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "",
                },
                new UserDto
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "",
                },
            };
            return Ok(users);
        }
    }
}
