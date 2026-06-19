using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;

namespace MoneyAPI.Repositories
{
    public class CartaoRepository : BaseRepository, ICartaoRepository
    {
        private readonly ApplicationContext _context;

        public CartaoRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cartao> GetCartaoById(int id, int usuarioId)
        {
            return await _context.Cartoes
                .Include(c => c.Conta)
                .Include(c => c.Lancamentos)
                .Where(co => co.Conta.UsuarioId == usuarioId)
                .Where(ca => ca.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cartao>> GetCartoes(int usuarioId)
        {
            return await _context.Cartoes
                .Include(c => c.Conta)
                .Include(c => c.Lancamentos)
                .Where(co => co.Conta.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Cartao> GetCartaoByNome(string nome, int usuarioId)
        {
            return await _context.Cartoes
                .Include(c => c.Conta)
                .Where(co => co.Conta.UsuarioId == usuarioId)
                .Where(ca => ca.Nome == nome)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Cartao>> GetCartoesFechados()
        {
            return await _context.Cartoes
                .Include(c => c.Conta)
                .Where(c => DateOnly.FromDateTime(DateTime.Now) > c.DataFechamento)
                .ToListAsync();
        }
    }
}