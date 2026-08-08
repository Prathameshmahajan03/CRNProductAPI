using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);

        Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);

        Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto);

    }
}