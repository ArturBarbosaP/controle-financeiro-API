using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Helpers;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using System.Text.RegularExpressions;

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
                .Include(x => x.ContaDestino)
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
                .Include(x => x.ContaDestino)
                .Where(x => x.UsuarioId == usuarioId)
                .Where(x => x.Data >= dataInicio && x.Data <= dataFim)
                .ToListAsync()
                .ContinueWith(t => t.Result
                    .OrderBy(x => x.Data)
                    .ThenBy(x => Utils.ordenacaoPadrao.ContainsKey(x.Tipo) ? Utils.ordenacaoPadrao[x.Tipo] : 99));

        }

        /// <summary>
        /// Uma fatura do cartão (indicado pelo nome na Descrição) na data de fechamento do cartão
        /// </summary>
        /// <param name="descricao"></param>
        /// <param name="data"></param>
        /// <param name="contaId"></param>
        /// <param name="categoriaFaturaId"></param>
        /// <param name="usuarioId"></param>
        /// <returns>Um lançamento da fatura do cartão</returns>
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

        /// <summary>
        /// Todas as faturas do cartão (indicado pelo nome na Descrição)
        /// </summary>
        /// <param name="descricao"></param>
        /// <param name="contaId"></param>
        /// <param name="categoriaFaturaId"></param>
        /// <param name="usuarioId"></param>
        /// <returns>Lista de lançamentos das faturas existentes para esse cartão</returns>
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
                    && l.Data >= lancamento.Data
                    && l.Fixo)
                .ToListAsync();
        }

        public async Task<List<Lancamento>> GetLancamentosPreLancamentoOld()
        {
            return await _context.Lancamentos
                .Include(l => l.Conta)
                .Where(l => l.PreLancamento && l.Data <= DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
        }

        /// <summary>
        /// Todos os lançamentos (menos os parcelados) na fatura do cartão
        /// </summary>
        /// <param name="cartaoId"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <returns>A soma de todos os valores dos lançamentos na fatura do cartão</returns>
        public Task<decimal> GetLancamentosNaFatura(int cartaoId, DateOnly dataInicio, DateOnly dataFim)
        {
            return _context.Lancamentos
                .Where(l => l.CartaoId == cartaoId
                    && !Regex.IsMatch(l.Descricao, @"^[0-9]+/[0-9]+")
                    && l.Data >= dataInicio
                    && l.Data <= dataFim)
                .SumAsync(l => l.Valor);
        }

        /// <summary>
        /// Todos os lançamentos parcelados na fatura do cartão
        /// </summary>
        /// <param name="cartaoId"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <returns>A soma de todos os valores dos lançamentos parcelados na fatura do cartão</returns>
        public Task<decimal> GetLancamentosParceladosNaFatura(int cartaoId, DateOnly dataInicio, DateOnly dataFim)
        {
            return _context.Lancamentos
                .Where(l => l.CartaoId == cartaoId
                    && Regex.IsMatch(l.Descricao, @"^[0-9]+/[0-9]+")
                    && l.Data >= dataInicio
                    && l.Data <= dataFim)
                .SumAsync(l => l.Valor);
        }
    }
}