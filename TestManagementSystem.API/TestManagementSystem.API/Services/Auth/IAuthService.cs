using TestManagementSystem.API.DTOs.Auth;

namespace TestManagementSystem.API.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
