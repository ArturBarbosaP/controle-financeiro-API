using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ILimiteRepository : IBaseRepository
    {
        Task<IEnumerable<Limite>> GetLimites(int usuarioId);

        Task<Limite> GetLimiteById(int id, int usuarioId);
    }
}