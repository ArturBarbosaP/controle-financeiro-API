using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Helpers;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;

namespace MoneyAPI.Repositories
{
    public class LancamentoRepository : BaseRepository, ILancamentoRepository
    {
        private readonly ApplicationContext _context;

        public LancamentoRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Lancamento> GetLancamentoById(int id, int usuarioId)
        {
            return await _context.Lancamentos
                .Include(x => x.Conta)
                .Include(x => x.Categoria)
                .Include(x => x.Cartao)
                .Where(x => x.UsuarioId == usuarioId)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosMensal(int usuarioId, int mes, int ano)
        {
            DateOnly dataInicio = new(ano, mes, 1);
            DateOnly dataFim = dataInicio.AddMonths(1).AddDays(-1);

            return await _context.Lancamentos
                .Include(x => x.Conta)
                .Include(x => x.Categoria)
                .Include(x => x.Cartao)
                .Where(x => x.UsuarioId == usuarioId)
                .Where(x => x.Data >= dataInicio && x.Data <= dataFim)
                .ToListAsync()
                .ContinueWith(t => t.Result
                    .OrderBy(x => x.Data)
                    .ThenBy(x => Utils.ordenacaoPadrao.ContainsKey(x.Tipo) ? Utils.ordenacaoPadrao[x.Tipo] : 99));

        }

        public async Task<Lancamento> GetLancamentoFaturaCartao(string descricao, DateOnly data, int contaId, int categoriaFaturaId, int usuarioId)
        {
            return await _context.Lancamentos
                .Where(u => u.UsuarioId == usuarioId)
                .Where(l => l.Descricao == descricao
                    && l.Data == data
                    && l.ContaId == contaId
                    && l.CategoriaId == categoriaFaturaId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosFaturasCartao(string descricao, int contaId, int categoriaFaturaId, int usuarioId)
        {
            return await _context.Lancamentos
                .Where(u => u.UsuarioId == usuarioId)
                .Where(l => l.Descricao == descricao
                    && l.ContaId == contaId
                    && l.CategoriaId == categoriaFaturaId)
                .ToListAsync();
        }

        public async Task<List<Lancamento>> GetLancamentosFixosByLancamento(Lancamento lancamento, int usuarioId)
        {
            return await _context.Lancamentos
                .Where(u => u.UsuarioId == usuarioId)
                .Where(l => l.ContaId == lancamento.ContaId
                    && l.ContaDestinoId == lancamento.ContaDestinoId
                    && l.CartaoId == lancamento.CartaoId
                    && l.CategoriaId == lancamento.CategoriaId
                    && l.Descricao == lancamento.Descricao
                    && l.Tipo == lancamento.Tipo
                    && l.Data >= lancamento.Data)
                .ToListAsync();
        }
    }
}