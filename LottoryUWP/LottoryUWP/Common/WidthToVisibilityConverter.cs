using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LottoryUWP.Common
{

    public class WidthToVisibilityConverter : IValueConverter
    {
    

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double v = 0;

            if (Double.TryParse(value.ToString(), out v))
            {
                return v == 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
