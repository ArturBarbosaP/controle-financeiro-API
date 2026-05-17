using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Usuario;

namespace MoneyAPI.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<ReadUsuarioDto>> GetUsuariosAsync();
        Task<ReadUsuarioDto?> GetUsuarioByIdAsync(int id);
        Task<ReadUsuarioDto?> GetUsuarioByNomeUsuarioAsync(string usuario);
        Task<ResponseDto> CreateAsync(AddUsuarioDto usuarioDto);
        Task<ResponseDto> UpdateAsync(int id, UpdateUsuarioDto usuarioDto);
        Task<ResponseDto> DeleteAsync(int id);
    }
}