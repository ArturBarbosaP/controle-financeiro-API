using static System.Net.Mime.MediaTypeNames;

namespace MoneyAPI.Models.Entities
{
    public class Cartao
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public DateOnly DataFechamento { get; set; }

        public DateOnly DataVencimento { get; set; }

        public decimal Limite { get; set; }

        public decimal LimiteDisponivel { get; set; }

        public decimal ValorParcelado { get; set; }

        public int ContaId { get; set; }

        public Conta Conta { get; set; }

        public List<Lancamento> Lancamentos { get; set; }
    }
}