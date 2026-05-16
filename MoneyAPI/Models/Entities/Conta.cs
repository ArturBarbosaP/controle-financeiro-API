namespace MoneyAPI.Models.Entities
{
    public class Conta : BaseEntity
    {
        public string Nome { get; set; }

        public decimal Saldo { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public List<Cartao> Cartoes { get; set; }

        public List<Lancamento> Lancamentos { get; set; }
    }
}