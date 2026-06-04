using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;

namespace MoneyAPI.Repositories
{
    public class ContaRepository : BaseRepository, IContaRepository
    {
        private readonly ApplicationContext _context;

        public ContaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Conta> GetContaById(int id, int usuarioId)
        {
            return await _context.Contas
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Conta>> GetContas(int usuarioId)
        {
            return await _context.Contas
                .Where(u => u.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Conta> GetContaByNome(string nome, int usuarioId)
        {
            return await _context.Contas
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Nome == nome)
                .FirstOrDefaultAsync();
        }
    }
}