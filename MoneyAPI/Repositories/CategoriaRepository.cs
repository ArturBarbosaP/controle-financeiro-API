using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Helpers;
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
                .ToListAsync()
                .ContinueWith(t => t.Result
                .OrderBy(c => Utils.ordenacaoPadrao.ContainsKey(c.Tipo) ? Utils.ordenacaoPadrao[c.Tipo] : 99));
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasPadroes(int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Padrao)
                .ToListAsync();
        }

        public async Task<Categoria> GetCategoriaPadraoFatura(int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Nome == "Pagamento de fatura" && c.Padrao)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasDeDespesa(int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Tipo == "Despesa")
                .ToListAsync();
        }

        public async Task<Categoria> GetCategoriaByNome(string nome, string tipo, int usuarioId)
        {
            return await _context.Categorias
                .Where(u => u.UsuarioId == usuarioId)
                .Where(c => c.Nome == nome && c.Tipo == tipo)
                .FirstOrDefaultAsync();
        }
    }
}