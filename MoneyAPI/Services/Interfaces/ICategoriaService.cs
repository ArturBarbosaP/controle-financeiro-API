using MoneyAPI.Models.DTOs;

namespace MoneyAPI.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetCategoriasAsync(int usuarioId);
        Task<CategoriaDto?> GetCategoriaByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(CategoriaDto categoriaDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, CategoriaDto categoriaDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
    }
}