using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    /// <summary>
    /// Provides authentication and token management business logic.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository used to access user data.</param>
        /// <param name="passwordHasher">Service used to hash and verify passwords.</param>
        /// <param name="jwtTokenGenerator">Service used to generate access and refresh tokens.</param>
        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Registers a new user and generates authentication tokens.
        /// </summary>
        /// <param name="dto">The user registration details.</param>
        /// <returns>An authentication response containing access and refresh tokens.</returns>
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already exists.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user);

            // Generate Refresh Token
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // Save Refresh Token in Database
            await _userRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = user.RefreshToken
            };
        }

        /// <summary>
        /// Authenticates a user using their email and password.
        /// </summary>
        /// <param name="dto">The user's login credentials.</param>
        /// <returns>
        /// An authentication response containing access and refresh tokens
        /// if the credentials are valid; otherwise, null.
        /// </returns>
        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            // Generate Access Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            // Generate Refresh Token
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // Save to Database
            await _userRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = user.RefreshToken
            };
        }

        /// <summary>
        /// Generates new access and refresh tokens using a valid refresh token.
        /// </summary>
        /// <param name="dto">The refresh token request.</param>
        /// <returns>
        /// A new authentication response if the refresh token is valid and has not expired;
        /// otherwise, null.
        /// </returns>
        public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(dto.RefreshToken);

            if (user == null)
                return null;

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            // Generate new tokens
            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // Rotate refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}