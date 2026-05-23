using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Categoria;

namespace MoneyAPI.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<ResponseCategoriaDto>> GetCategoriasAsync(int usuarioId);
        Task<ResponseCategoriaDto?> GetCategoriaByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(RequestCategoriaDto categoriaDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, RequestCategoriaDto categoriaDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}