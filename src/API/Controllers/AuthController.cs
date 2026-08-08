using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Provides endpoints for user registration, login, and token refresh.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">
        /// Authentication service used to handle user authentication operations.
        /// </param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">The registration details.</param>
        /// <returns>The result of the registration operation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Authenticates a user and generates authentication tokens.
        /// </summary>
        /// <param name="dto">The user's login credentials.</param>
        /// <returns>
        /// Authentication tokens if the credentials are valid;
        /// otherwise, an unauthorized response.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized("Invalid email or password.");

            return Ok(result);
        }

        /// <summary>
        /// Generates new authentication tokens using a refresh token.
        /// </summary>
        /// <param name="dto">The refresh token request.</param>
        /// <returns>
        /// New authentication tokens if the refresh token is valid;
        /// otherwise, an unauthorized response.
        /// </returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            if (result == null)
                return Unauthorized("Invalid or expired refresh token.");

            return Ok(result);
        }
    }
}