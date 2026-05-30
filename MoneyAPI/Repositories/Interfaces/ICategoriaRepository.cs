using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IBaseRepository
    {
        void AddCategoriasPadrao(int usuarioId);

        Task DeleteCategoriasPadrao(int usuarioId);

        Task<IEnumerable<Categoria>> GetCategorias(int usuarioId);

        Task<Categoria> GetCategoriaById(int id, int usuarioId);

        Task<Categoria> GetCategoriaByIdTipo(int id, string tipo, int usuarioId);

        Task<IEnumerable<Categoria>> GetCategoriasDeDespesa(int usuarioId);
    }
}