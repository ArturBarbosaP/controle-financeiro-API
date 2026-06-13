using System.Collections.Concurrent;

namespace MoneyAPI.Data
{
    public class Notification
    {
        private readonly ConcurrentDictionary<int, List<string>> _notificacoes = new();

        public void Insert(int usuarioId, string notificacao)
        {
            _notificacoes.AddOrUpdate(usuarioId, new List<string> { notificacao }, (_,lista) => { lista.Add(notificacao); return lista; });
        }

        public List<string> GetAll(int usuarioId)
        {
            return _notificacoes.TryGetValue(usuarioId, out List<string> notificacoes) ? notificacoes : [];
        }

        public void DeleteAll(int usuarioId)
        {
            _notificacoes.TryRemove(usuarioId, out _);
        }
    }
}