using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Limite;

namespace MoneyAPI.Services.Interfaces
{
    public interface ILimiteService
    {
        Task<IEnumerable<ResponseLimiteDto>> GetLimitesAsync(int usuarioId, int mes, int ano);
        Task<ResponseLimiteDto?> GetLimiteByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(RequestLimiteDto limiteDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, RequestLimiteDto limiteDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}