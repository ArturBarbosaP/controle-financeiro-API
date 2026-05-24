using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;

namespace MoneyAPI.Repositories
{
    public class CategoriaRepository : BaseRepository, ICategoriaRepository
    {
        private readonly ApplicationContext _context;

        public CategoriaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Categoria> GetCategoriaById(int id, int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Categoria> GetCategoriaByIdTipo(int id, string tipo, int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Tipo == tipo)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Categoria>> GetCategorias(int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasDeDespesa(int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Tipo == "Despesa")
                .ToListAsync();
        }
    }
}