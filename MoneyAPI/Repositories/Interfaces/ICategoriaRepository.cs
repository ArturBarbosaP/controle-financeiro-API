using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IBaseRepository
    {
        Task<IEnumerable<Categoria>> GetCategorias(int usuarioId);

        Task<Categoria> GetCategoriaById(int id, int usuarioId);

        Task<Categoria> GetCategoriaByIdTipo(int id, string tipo, int usuarioId);

        Task<IEnumerable<Categoria>> GetCategoriasPadroes(int usuarioId);

        Task<IEnumerable<Categoria>> GetCategoriasDeDespesa(int usuarioId);

        Task<Categoria> GetCategoriaPadraoFatura(int usuarioId);

        Task<Categoria> GetCategoriaByNome(string nome, string tipo, int usuarioId);
    }
}