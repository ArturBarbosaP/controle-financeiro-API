using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Usuario;

namespace MoneyAPI.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<ResponseUsuarioDto>> GetUsuariosAsync();
        Task<ResponseUsuarioDto?> GetUsuarioByIdAsync(int id);
        Task<ResponseUsuarioDto?> GetUsuarioByNomeUsuarioAsync(string usuario);
        Task<ResponseDto> CreateAsync(RequestAddUsuarioDto usuarioDto);
        Task<ResponseDto> UpdateAsync(int id, RequestUpdateUsuarioDto usuarioDto);
        Task<ResponseDto> UpdatePasswordAsync(int id, RequestPasswordUpdateUsuarioDto passwordDto);
        Task<ResponseDto> DeleteAsync(int id, string token);
    }
}