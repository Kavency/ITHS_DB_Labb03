using System.Globalization;
using System.Windows.Data;

namespace ITHS_DB_Labb03.Core
{
    internal class ValidationToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values) 
            { 
                if (value is bool hasError && hasError) 
                { 
                    return false; 
                } 
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
