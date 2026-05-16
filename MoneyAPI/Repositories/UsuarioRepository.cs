using Microsoft.EntityFrameworkCore;
using MoneyAPI.Data;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;

namespace MoneyAPI.Repositories
{
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        private readonly ApplicationContext _context;

        public UsuarioRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            return await _context.Usuarios
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetUsuarioByNomeUsuario(string usuario)
        {
            return await _context.Usuarios
                .Where(x => x.NomeUsuario.Equals(usuario))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            return await _context.Usuarios
                .ToListAsync();
        }
    }
}