using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MoneyAPI.Helpers.Attributes
{
    public class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly uint _precisao;

        public DecimalPrecisionAttribute(uint precision)
        {
            _precisao = precision;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !decimal.TryParse(value.ToString(), out decimal valorDecimal))
                return ValidationResult.Success;

            string valueStr = valorDecimal.ToString(CultureInfo.InvariantCulture);

            if (valueStr.IndexOf('.') == -1)
                return ValidationResult.Success;

            if (valueStr.Split('.')[1].Length <= _precisao)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"O {validationContext.DisplayName} aceita apenas {_precisao} casas decimais!");
        }
    }
}