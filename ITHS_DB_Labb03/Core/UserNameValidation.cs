using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;


namespace ITHS_DB_Labb03.Core
{
    internal class UserNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value is not null)
            {
                string valueString = value.ToString();
                using var db = new TodoDbContext();

                var user = db.Users.FirstOrDefault(x => x.UserName == valueString);

                if (user is not null && valueString == user.UserName)
                {
                    Debug.WriteLine("Username already exists.");
                    return new ValidationResult(false, "Username already exists!");
                }
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Enter username!");
        }
    }
}
