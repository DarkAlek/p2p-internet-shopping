using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using client_desktop.Model.Models;

namespace client_desktop.Converters
{
    class ObjectToTypeStringConverter : IValueConverter
    {
        public object Convert(
         object value, Type targetType,
         object parameter, System.Globalization.CultureInfo culture)
        {
            UserApi val = (UserApi)value;
            if(val.Type == 0)
            {
                return "Customer";
            }
            else if(val.Type == 1)
            {
                return "Provider";
            }
            return "Admin";
            //return value == null ? null: value.GetType().Name.Split('_')[0];
        }

        public object ConvertBack(
         object value, Type targetType,
         object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
