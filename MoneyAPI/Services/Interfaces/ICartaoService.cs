using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Cartao;

namespace MoneyAPI.Services.Interfaces
{
    public interface ICartaoService
    {
        Task<IEnumerable<ResponseCartaoDto>> GetCartoesAsync(int usuarioId);
        Task<ResponseCartaoDto?> GetCartaoByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(RequestCartaoDto cartaoDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, RequestCartaoDto cartaoDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}