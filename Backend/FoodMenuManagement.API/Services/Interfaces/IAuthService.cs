
using FoodMenuManagement.API.DTOs.Auth;

namespace FoodMenuManagement.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request);

        Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
