namespace MoneyAPI.Models.Entities
{
    public class Lancamento
    {
        public int Id { get; set; }
        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public string Descricao { get; set; }

        public DateOnly Data { get; set; }

        public string? Observacao { get; set; }

        public bool Fixo { get; set; }

        public bool PreLancamento { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public int ContaId { get; set; }

        public Conta Conta { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int? CartaoId { get; set; }

        public Cartao Cartao { get; set; }
    }
}