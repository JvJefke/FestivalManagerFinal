using FestivalManager_2.Model;
using FestivalManager_2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FestivalManager_2.View.Converters
{
    class IsOptredenToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Optreden o = (Optreden)values[0];
            OptredenUurVM ou = null;
            if(values[1] != System.Windows.DependencyProperty.UnsetValue)
                ou = (OptredenUurVM)values[1];

            if(o == null)
                return Color.FromArgb(0, 0, 0, 0);
            if (ou != null && o.ID == ou.Optreden.ID)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B46"));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#96D1D4"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
