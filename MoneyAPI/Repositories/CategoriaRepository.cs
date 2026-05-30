using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MoneyAPI.Repositories
{
    public class CategoriaRepository : BaseRepository, ICategoriaRepository
    {
        private readonly ApplicationContext _context;

        private readonly List<Categoria> categoriasDoSistema =
        [
            new Categoria
            {
                Nome = "Pagamento de fatura",
                Tipo = "Despesa",
                Cor = "#BCBCBC"
            },
            new Categoria
            {
                Nome = "Transferência",
                Tipo = "Transf.",
                Cor = "#BCBCBC"
            }
        ];

        public CategoriaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public void AddCategoriasPadrao(int usuarioId)
        {
            foreach (Categoria categoria in categoriasDoSistema)
            {
                categoria.UsuarioId = usuarioId;
                Add(categoria);
            }
        }

        public async Task DeleteCategoriasPadrao(int usuarioId)
        {
            foreach (Categoria categoria in categoriasDoSistema)
            {
                Categoria categoriaDelete = await _context.Categorias
                    .Where(u => u.UsuarioId == usuarioId)
                    .Where(c => c.Nome == categoria.Nome && c.Tipo == categoria.Tipo && c.Cor == categoria.Cor)
                    .FirstAsync();

                Delete(categoriaDelete);
            }
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