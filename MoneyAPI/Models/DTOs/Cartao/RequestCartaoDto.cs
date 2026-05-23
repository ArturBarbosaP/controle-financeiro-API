using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Cartao
{
    public class RequestCartaoDto
    {
        [Required(ErrorMessage = "Digite o nome do cartão!")]
        [MaxLength(80, ErrorMessage = "O nome do cartão não pode ultrapassar 80 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Escolha a data de fechamento do cartão!")]
        public DateOnly DataFechamento { get; set; }

        [Required(ErrorMessage = "Escolha a data de vencimento do cartão!")]
        public DateOnly DataVencimento { get; set; }

        [Required(ErrorMessage = "Digite o limite do cartão!")]
        [DecimalPrecision(2)]
        public decimal Limite { get; set; }

        [Required(ErrorMessage = "Digite o limite disponível do cartão!")]
        [DecimalPrecision(2)]
        public decimal LimiteDisponivel { get; set; }

        [Required(ErrorMessage = "Digite o valor parcelado do cartão!")]
        [DecimalPrecision(2)]
        public decimal ValorParcelado { get; set; }

        [Required(ErrorMessage = "Escolha a conta do cartão!")]
        public int ContaId { get; set; }
    }
}