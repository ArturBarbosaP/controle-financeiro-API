using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MoneyAPI.Helpers.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public bool RequerMaiuscula { get; set; } = true;
        public bool RequerMinuscula { get; set; } = true;
        public bool RequerNumero { get; set; } = true;
        public bool RequerCaractereEspecial { get; set; } = true;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            string senha = value.ToString();
            List<string> erros = [];

            if (RequerMaiuscula && !Regex.IsMatch(senha, @"[A-Z]"))
                erros.Add("uma letra maiúscula");

            if (RequerMinuscula && !Regex.IsMatch(senha, @"[a-z]"))
                erros.Add("uma letra minúscula");

            if (RequerNumero && !Regex.IsMatch(senha, @"[0-9]"))
                erros.Add("um número");

            if (RequerCaractereEspecial && !Regex.IsMatch(senha, @"[!@#$%^&*()]"))
                erros.Add("um caractere especial");

            if (erros.Any())
            {
                string mensagem = $"A sua senha deve conter pelo menos {string.Join(", ", erros)}.";
                return new ValidationResult(mensagem);
            }

            return ValidationResult.Success;
        }
    }
}