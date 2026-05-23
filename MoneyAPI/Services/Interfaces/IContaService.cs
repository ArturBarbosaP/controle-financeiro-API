using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Conta;

namespace MoneyAPI.Services.Interfaces
{
    public interface IContaService
    {
        Task<IEnumerable<ResponseContaDto>> GetContasAsync(int usuarioId);
        Task<ResponseContaDto?> GetContaByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(RequestContaDto contaDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, RequestContaDto contaDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}