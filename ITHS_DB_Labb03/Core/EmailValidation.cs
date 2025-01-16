using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ITHS_DB_Labb03.Core
{
    class EmailValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (value is null || !regex.IsMatch(value.ToString()))
                return new ValidationResult(false, "Not a valid email address!");

            return ValidationResult.ValidResult;
        }
    }
}
