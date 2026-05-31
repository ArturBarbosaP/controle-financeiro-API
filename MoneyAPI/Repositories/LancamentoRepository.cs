using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;

namespace MoneyAPI.Repositories
{
    public class LancamentoRepository : BaseRepository, ILancamentoRepository
    {
        private readonly ApplicationContext _context;
        private readonly Dictionary<string, int> ordem = new()
        {
            { "Receita", 1 },
            { "Transf.", 2 },
            { "Despesa", 3 }
        };

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
                    .ThenBy(x => ordem.ContainsKey(x.Tipo) ? ordem[x.Tipo] : 99));

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
    }
}