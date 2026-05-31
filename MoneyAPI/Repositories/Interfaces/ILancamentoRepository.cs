using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ILancamentoRepository : IBaseRepository
    {
        Task<IEnumerable<Lancamento>> GetLancamentosMensal(int usuarioId, int mes, int ano);

        Task<Lancamento> GetLancamentoById(int id, int usuarioId);

        Task<Lancamento> GetLancamentoFaturaCartao(string descricao, DateOnly data, int contaId, int categoriaFaturaId, int usuarioId);
    }
}