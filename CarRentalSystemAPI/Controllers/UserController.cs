using CarRentalSystemAPI.Models;
using CarRentalSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // POST /api/users/register
        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationModel registrationModel)
        {
            _logger.LogInformation("Attempting to register a new user.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid user registration data received.");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Name = registrationModel.Name,
                Email = registrationModel.Email,
                Password = registrationModel.Password,
                Role = registrationModel.Role
            };

            try
            {
                _userService.RegisterUser(user);
                _logger.LogInformation("User registered successfully with Email: {Email}", user.Email);
                return Ok("User registered successfully.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error registering user with Email: {Email}", user.Email);
                return BadRequest(ex.Message);
            }
        }

        // POST /api/users/login
        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel loginModel)
        {
            _logger.LogInformation("User attempting to log in with Email: {Email}", loginModel.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login data received.");
                return BadRequest(ModelState);
            }

            var token = _userService.AuthenticateUser(loginModel.Email, loginModel.Password);

            if (token == null)
            {
                _logger.LogWarning("Authentication failed for Email: {Email}", loginModel.Email);
                return Unauthorized("Invalid email or password.");
            }

            _logger.LogInformation("User authenticated successfully with Email: {Email}", loginModel.Email);
            return Ok(new { Token = token });
        }
    }

    // DTOs for User Registration and Login
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "User name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required]
        [RegularExpression("Admin|User", ErrorMessage = "Role must be either 'Admin' or 'User'.")]
        public string Role { get; set; } = "User"; // "Admin" or "User"
    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "User email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
