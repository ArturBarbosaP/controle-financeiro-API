using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Usuario
{
    public class RequestUpdateUsuarioDto
    {
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres!")]
        public string Nome { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "O usuário deve ter entre 5 e 100 caracteres!")]
        public string NomeUsuario { get; set; }
    }
}