using MoneyAPI.Models.Entities;

namespace MoneyAPI.Repositories.Interfaces
{
    public interface ILancamentoRepository : IBaseRepository
    {
        Task<IEnumerable<Lancamento>> GetLancamentosMensal(int usuarioId, int mes, int ano);

        Task<Lancamento> GetLancamentoById(int id, int usuarioId);

        Task<Lancamento> GetLancamentoFaturaCartao(string descricao, DateOnly data, int contaId, int categoriaFaturaId, int usuarioId);

        Task<IEnumerable<Lancamento>> GetLancamentosFaturasCartao(string descricao, int contaId, int categoriaFaturaId, int usuarioId);

        Task<List<Lancamento>> GetLancamentosFixosByLancamento(Lancamento lancamento, int usuarioId);

        Task<List<Lancamento>> GetLancamentosPreLancamentoOld();

        Task<decimal> GetLancamentosParceladosNaFatura(int cartaoId, DateOnly dataInicio, DateOnly dataFim);

        Task<decimal> GetLancamentosNaFatura(int cartaoId, DateOnly dataInicio, DateOnly dataFim);

        Task<decimal> GetSaldoAcumulado(DateOnly data, int usuarioId);
    }
}