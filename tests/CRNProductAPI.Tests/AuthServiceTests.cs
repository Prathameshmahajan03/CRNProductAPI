using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace CRNProductAPI.Tests
{
    public class AuthServiceTests
    {

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {

            // Arrange
            var loginDto = new LoginRequestDto
            {
                Email = "testuser@gmail.com",
                Password = "Password@123"
            };

            var user = new User
            {
                Id = 1,
                FullName = "Test User",
                Email = "testuser@gmail.com",
                PasswordHash = "hashed-password",
                Role = "User"
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.VerifyPassword(loginDto.Password, user.PasswordHash))
                .Returns(true);

            _jwtTokenGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns("fake-jwt-token");

            // Act
            var authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object);

            var result = await authService.LoginAsync(loginDto);


            // Assert
            Assert.NotNull(result);
            Assert.Equal("fake-jwt-token", result.Token);

            _userRepositoryMock.Verify(
                r => r.GetByEmailAsync(loginDto.Email),
                Times.Once);

            _passwordHasherMock.Verify(
                p => p.VerifyPassword(loginDto.Password, user.PasswordHash),
                Times.Once);

            _jwtTokenGeneratorMock.Verify(
                j => j.GenerateToken(user),
                Times.Once);

        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnNewTokens_WhenRefreshTokenIsValid()
        {

            // Arrange
            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "old-refresh-token"
            };

            var user = new User
            {
                Id = 1,
                FullName = "Test User",
                Email = "testuser@gmail.com",
                PasswordHash = "hashed-password",
                Role = "User",
                RefreshToken = "old-refresh-token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };


            _userRepositoryMock
                .Setup(r => r.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(user);

            _jwtTokenGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns("new-access-token");

            _jwtTokenGeneratorMock
                .Setup(j => j.GenerateRefreshToken())
                .Returns("new-refresh-token");

            _userRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            // Act
            var authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object);

            var result = await authService.RefreshTokenAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new-access-token", result.Token);
            Assert.Equal("new-refresh-token", result.RefreshToken);

            _userRepositoryMock.Verify(
                r => r.GetByRefreshTokenAsync(request.RefreshToken),
                Times.Once);

            _jwtTokenGeneratorMock.Verify(
                j => j.GenerateToken(user),
                Times.Once);

            _jwtTokenGeneratorMock.Verify(
                j => j.GenerateRefreshToken(),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);


        }
    }
}