using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs
{
    public class ContaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome da conta!")]
        [MaxLength(80, ErrorMessage = "O nome da conta não pode ultrapassar 80 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o saldo da conta!")]
        [DecimalPrecision(2)]
        public decimal Saldo { get; set; }
    }
}