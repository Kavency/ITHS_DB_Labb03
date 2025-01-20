using ITHS_DB_Labb03.Model;
using ITHS_DB_Labb03.ViewModel;
using MongoDB.Driver;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;


namespace ITHS_DB_Labb03.Core
{
    internal class UserNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not null)
            {
                var db = new MongoClient(MainViewModel.connectionString);
                var users = db.GetDatabase("todoapp").GetCollection<User>("Users");
                string valueString = value.ToString();

                var user = users.AsQueryable().FirstOrDefault(x => x.UserName == valueString);
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
