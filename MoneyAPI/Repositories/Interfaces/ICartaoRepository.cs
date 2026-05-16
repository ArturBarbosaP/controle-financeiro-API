using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ICartaoRepository : IBaseRepository
    {
        Task<IEnumerable<Cartao>> GetCartoes(int usuarioId);

        Task<Cartao> GetCartaoById(int id, int usuarioId);
    }
}