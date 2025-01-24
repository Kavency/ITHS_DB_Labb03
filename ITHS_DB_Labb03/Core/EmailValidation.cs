using ITHS_DB_Labb03.Model;
using ITHS_DB_Labb03.ViewModel;
using MongoDB.Driver;
using System.Diagnostics;
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
            
            if(value is not null)
            {
                string valueString = value.ToString();

                using var db = new MongoClient(MainViewModel.ConnectionString);
                var users = db.GetDatabase(MainViewModel.DbName).GetCollection<User>("Users");

                var user = users.AsQueryable().FirstOrDefault(x => x.Email == valueString);
                
                if (value is null || !regex.IsMatch(value.ToString()))
                {
                    Debug.WriteLine("Not a vaild email address.");
                    return new ValidationResult(false, "Not a valid email address!");
                }
                if (user is not null && user.Email == valueString)
                {
                    Debug.WriteLine("Email address already exist!");
                    return new ValidationResult(false, "Email address already exists!");
                }
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Enter a valid email address!");
        }
    }
}
