using System.Globalization;
using System.Windows.Controls;

namespace ITHS_DB_Labb03.Core;

internal class NotEmptyStringValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return new ValidationResult(false, "Field cannot be empty!");

        return ValidationResult.ValidResult;
    }
}
