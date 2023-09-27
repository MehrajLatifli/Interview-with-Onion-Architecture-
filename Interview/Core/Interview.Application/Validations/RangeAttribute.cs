using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Validations
{
    public class RangeAttribute : ValidationAttribute
    {
        private readonly double _minValue;
        private readonly double _maxValue;

        public RangeAttribute(double minValue, double maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is double number)
            {
                if (number >= _minValue && number <= _maxValue)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"The {validationContext.DisplayName} must be between {_minValue} and {_maxValue}.");
                }
            }

            return new ValidationResult("Invalid input type. This attribute is only applicable to double numbers.");
        }
    }

}
