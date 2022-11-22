using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace gApp2
{
    class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                float val = (float)value;
                switch ((string)parameter)
                {
                    case "BG":
                        if (val < 0)
                            return "#ffbb88";
                        if (val == 0)
                            return "#bbddff";
                        if (val > 0)
                            return "#bbff99";
                        break;
                    case "Text":
                        if (val < 0)
                            return "Red";
                        if (val == 0)
                            return "#bbddff";
                        if (val > 0)
                            return "Green";
                        break;
                    default:
                        return "#bbddff";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
