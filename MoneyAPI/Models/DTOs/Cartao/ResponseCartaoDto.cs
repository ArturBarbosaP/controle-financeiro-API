namespace MoneyAPI.Models.DTOs.Cartao
{
    public class ResponseCartaoDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateOnly DataFechamento { get; set; }

        public DateOnly DataVencimento { get; set; }

        public decimal Limite { get; set; }

        public decimal LimiteDisponivel { get; set; }

        public decimal ValorParcelado { get; set; }

        public int ContaId { get; set; }

        public string ContaNome { get; set; }
    }
}