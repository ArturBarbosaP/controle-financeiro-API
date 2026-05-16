using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository
    {
        Task<IEnumerable<Usuario>> GetUsuarios();

        Task<Usuario> GetUsuarioById(int id);

        Task<Usuario> GetUsuarioByNomeUsuario(string usuario);
    }
}