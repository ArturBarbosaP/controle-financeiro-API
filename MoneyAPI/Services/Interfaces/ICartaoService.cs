using MoneyAPI.Models.DTOs;

namespace MoneyAPI.Services.Interfaces
{
    public interface ICartaoService
    {
        Task<IEnumerable<CartaoDto>> GetCartoesAsync(int usuarioId);
        Task<CartaoDto?> GetCartaoByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(CartaoDto cartaoDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, CartaoDto cartaoDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}