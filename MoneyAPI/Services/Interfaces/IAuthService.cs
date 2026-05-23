using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Auth;

namespace MoneyAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(RequestLoginDto loginDTO);
        Task<ResponseDto> LogoutAsync(string token);
        Task<ResponseDto> VerifyAsync(string token);
    }
}