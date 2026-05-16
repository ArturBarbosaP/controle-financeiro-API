using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome da categoria!")]
        [MaxLength(80, ErrorMessage = "O nome da categoria não pode ultrapassar 80 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Selecione o tipo da categoria!")]
        [InList(["Despesa", "Receita", "Trasnf."], ErrorMessage = "O tipo da categoria pode ser apenas Despesa, Receita ou Transferência!")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Selecione a cor da categoria!")]
        [Length(7, 7, ErrorMessage = "Apenas cores em hexadecimal são aceitas!")]
        [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Apenas cores em hexadecimal são aceitas!")]
        public string Cor { get; set; }
    }
}