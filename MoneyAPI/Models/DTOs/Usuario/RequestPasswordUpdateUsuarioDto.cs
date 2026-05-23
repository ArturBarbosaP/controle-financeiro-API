using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Usuario
{
    public class RequestPasswordUpdateUsuarioDto
    {
        [Required(ErrorMessage = "Digite a senha atual!")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Digite a nova senha!")]
        [MinLength(8, ErrorMessage = "Sua nova senha deve conter no mínimo 8 caracteres!")]
        [StrongPassword]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Confirme a nova senha!")]
        [Compare(nameof(NovaSenha), ErrorMessage = "As senhas devem ser iguais!")]
        public string ConfirmarNovaSenha { get; set; }
    }
}