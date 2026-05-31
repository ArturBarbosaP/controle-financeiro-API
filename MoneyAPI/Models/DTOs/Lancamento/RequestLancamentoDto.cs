using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Lancamento
{
    public class RequestLancamentoDto : IValidatableObject
    {
        [Required(ErrorMessage = "Selecione o tipo do lançamento!")]
        [InList(["Despesa", "Receita", "Transf."], ErrorMessage = "O tipo do lançamento pode ser apenas Transf., Despesa ou Receita!")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Digite o valor do lançamento!")]
        [DecimalPrecision(2)]
        [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser positivo!")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Digite a descrição do lançamento!")]
        [MaxLength(100, ErrorMessage = "A descrição do lançamento não pode ultrapassar 100 caracteres!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Escolha a data do lançamento!")]
        public DateOnly Data { get; set; }

        [MaxLength(1000, ErrorMessage = "A observação do lançamento não pode ultrapassar 1000 caracteres!")]
        public string? Observacao { get; set; }

        public bool Fixo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A parcela deve ser positiva!")]
        public int Parcelas { get; set; }

        public bool PreLancamento { get; set; }

        [Required(ErrorMessage = "Selecione a categoria do lançamento!")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Selecione a conta do lançamento!")]
        public int ContaId { get; set; }

        public int? CartaoId { get; set; }

        public int? ContaDestinoId { get; set; }

        //validações
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool transferencia = ContaDestinoId.HasValue && ContaDestinoId.Value != 0;

            if ((transferencia && Tipo != "Transf.") || (Tipo == "Transf." && !transferencia))
                yield return new ValidationResult("Preencha corretamente os campos para lançamento do tipo Transferência!", new[] { nameof(ContaDestinoId), nameof(Tipo) });

            if (Fixo && Parcelas > 1)
                yield return new ValidationResult("O lançamento não pode ser parcelado e fixo!", new[] { nameof(Parcelas), nameof(Fixo) });

            if (Tipo == "Receita" && (CartaoId.HasValue && CartaoId.Value != 0))
                yield return new ValidationResult("O lançamento do tipo Receita não pode ter cartão!", new[] { nameof(CartaoId) });

            if (transferencia)
            {
                if (CartaoId.HasValue && CartaoId.Value != 0)
                    yield return new ValidationResult("O lançamento do tipo Transferência não pode ter cartão!", new[] { nameof(CartaoId) });

                if (ContaDestinoId == ContaId)
                    yield return new ValidationResult("As contas de origem e destino não podem ser iguais!", new[] { nameof(ContaId), nameof(ContaDestinoId) });
            }
        }
    }
}