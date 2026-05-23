using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Usuário ou senha inválidos!")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Usuário ou senha inválidos!")]
        public string Senha { get; set; }
    }
}