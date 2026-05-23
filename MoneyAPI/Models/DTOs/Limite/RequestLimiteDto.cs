using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Limite
{
    public class RequestLimiteDto
    {
        [Required(ErrorMessage = "Digite o valor do limite!")]
        [DecimalPrecision(2)]
        [Range(1, double.MaxValue, ErrorMessage = "O valor do limite deve ser positivo!")]
        public decimal ValorLimite { get; set; }

        [Required(ErrorMessage = "Escolha a categoria do limite!")]
        public int CategoriaId { get; set; }
    }
}