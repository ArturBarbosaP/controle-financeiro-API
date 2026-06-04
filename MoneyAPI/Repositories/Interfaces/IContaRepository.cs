using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface IContaRepository : IBaseRepository
    {
        Task<IEnumerable<Conta>> GetContas(int usuarioId);

        Task<Conta> GetContaById(int id, int usuarioId);

        Task<Conta> GetContaByNome(string nome, int usuarioId);
    }
}