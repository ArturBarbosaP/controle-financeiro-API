using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs
{
    public class LancamentoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o tipo do lançamento!")]
        [InList(["Despesa", "Receita", "Transf."], ErrorMessage = "O tipo do lançamento pode ser apenas Despesa ou Receita!")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Digite o valor do lançamento!")]
        [DecimalPrecision(2)]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Digite a descrição do lançamento!")]
        [MaxLength(100, ErrorMessage = "A descrição do lançamento não pode ultrapassar 100 caracteres!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Escolha a data do lançamento!")]
        public DateOnly Data { get; set; }

        [MaxLength(1000, ErrorMessage = "A observação do lançamento não pode ultrapassar 1000 caracteres!")]
        public string? Observacao { get; set; }

        public bool Fixo { get; set; }

        public int? Parcelas { get; set; }

        public bool PreLancamento { get; set; }

        [Required(ErrorMessage = "Selecione a categoria do lançamento!")]
        public int CategoriaId { get; set; }

        public string? CategoriaNome { get; set; }

        [Required(ErrorMessage = "Selecione a conta do lançamento!")]
        public int ContaId { get; set; }

        public string? ContaNome { get; set; }

        public int UsuarioId { get; set; }

        public int? CartaoId { get; set; }
        public string? CartaoNome { get; set; }
        public int? ContaDestinoId { get; set; }
    }
}