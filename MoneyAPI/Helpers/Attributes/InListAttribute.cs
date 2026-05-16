using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Helpers.Attributes
{
    public class InListAttribute : ValidationAttribute
    {
        private readonly List<string> _values;

        public InListAttribute(string[] values)
        {
            _values = values.ToList();
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            if (_values.Contains(value.ToString()))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} precisa conter apenas {string.Join(" ,", _values)}");
        }
    }
}