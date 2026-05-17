using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Usuario
{
    public class AddUsuarioDto
    {
        [Required(ErrorMessage = "Digite o nome!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o usuário!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O usuário deve ter entre 5 e 100 caracteres!")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Digite a senha!")]
        [MinLength(8, ErrorMessage = "Sua senha deve conter no mínimo 8 caracteres!")]
        [StrongPassword]
        public string? Senha { get; set; }

        [Required(ErrorMessage = "Confirme a senha!")]
        [Compare(nameof(Senha), ErrorMessage = "As senhas devem ser iguais!")]
        public string? ConfirmarSenha { get; set; }
    }
}