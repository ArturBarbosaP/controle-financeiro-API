using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs
{
    public class LimiteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o valor do limite!")]
        [DecimalPrecision(2)]
        public decimal ValorLimite { get; set; }

        [Required(ErrorMessage = "Escolha a categoria do limite!")]
        public int CategoriaId { get; set; }

        public string? CategoriaNome { get; set; }
    }
}