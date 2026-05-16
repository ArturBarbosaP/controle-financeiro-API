using MoneyAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o usuário!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O usuário deve ter entre 5 e 100 caracteres!")]
        public string NomeUsuario { get; set; }

        [MinLength(8, ErrorMessage = "Sua senha deve conter no mínimo 8 caracteres!")]
        [StrongPassword]
        public string? Senha { get; set; }

        [Compare(nameof(Senha), ErrorMessage = "As senhas devem ser iguais!")]
        public string? ConfirmarSenha { get; set; }

        public string? SenhaAtual { get; set; }
    }
}