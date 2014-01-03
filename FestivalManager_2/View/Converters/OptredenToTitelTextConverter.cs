﻿using FestivalManager_2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FestivalManager_2.View.Converters
{
    class OptredenToTitelTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Optreden o = (Optreden)value;
            if (o != null && o.ID != 0)
                return "( Bestaand )";
            else
                return "( Nieuw )";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
