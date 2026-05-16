using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IBaseRepository
    {
        Task<IEnumerable<Categoria>> GetCategorias(int usuarioId);

        Task<Categoria> GetCategoriaById(int id, int usuarioId);

        Task<IEnumerable<Categoria>> GetCategoriasDeDespesa(int usuarioId);
    }
}