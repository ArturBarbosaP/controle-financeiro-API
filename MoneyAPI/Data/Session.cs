using System.Collections.Concurrent;

namespace MoneyAPI.Data
{
    public class Session
    {
        private readonly ConcurrentDictionary<string, int> _sessoes = new();
        private readonly List<int> adminsUsuarioId = new() { 1 };

        public string CriarSessao(int usuarioId)
        {
            var token = Guid.NewGuid().ToString();
            _sessoes[token] = usuarioId;
            return token;
        }

        public int? ObterUsuarioId(string token)
        {
            return _sessoes.TryGetValue(token, out var id) ? id : null;
        }

        public bool SessaoExiste(string token)
        {
            return _sessoes.ContainsKey(token);
        }

        public void RemoverSessao(string token)
        {
            _sessoes.TryRemove(token, out _);
        }

        public bool UsuarioInAdminList(string token)
        {
            return _sessoes.TryGetValue(token, out var id) && adminsUsuarioId.Contains(id);
        }
    }
}