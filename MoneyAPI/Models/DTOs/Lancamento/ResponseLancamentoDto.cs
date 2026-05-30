namespace MoneyAPI.Models.DTOs.Lancamento
{
    public class ResponseLancamentoDto
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public string Descricao { get; set; }

        public DateOnly Data { get; set; }

        public string Observacao { get; set; }

        public bool Fixo { get; set; }

        public bool PreLancamento { get; set; }

        public int CategoriaId { get; set; }

        public string CategoriaNome { get; set; }

        public int ContaId { get; set; }

        public string ContaNome { get; set; }

        public int CartaoId { get; set; }

        public string CartaoNome { get; set; }

        public int ContaDestinoId { get; set; }

        public string ContaDestinoNome { get; set; }
    }
}