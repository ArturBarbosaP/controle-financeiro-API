using MoneyAPI.Models.DTOs;

namespace MoneyAPI.Services.Interfaces
{
    public interface IContaService
    {
        Task<IEnumerable<ContaDto>> GetContasAsync(int usuarioId);
        Task<ContaDto?> GetContaByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(ContaDto contaDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, ContaDto contaDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}