using MoneyAPI.Models.DTOs;

namespace MoneyAPI.Services.Interfaces
{
    public interface ILimiteService
    {
        Task<IEnumerable<LimiteDto>> GetLimitesAsync(int usuarioId);
        Task<LimiteDto?> GetLimiteByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(LimiteDto limiteDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, LimiteDto limiteDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}