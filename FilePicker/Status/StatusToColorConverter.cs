using FilePicker.Status;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FilePicker.Status
{
    class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ApplicationStatusEnum)value;
            if (status == ApplicationStatusEnum.Ready)
            {
                return new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)200, (byte)20));
            }
            if (status == ApplicationStatusEnum.OldDataFromDb)
            {
                return new SolidColorBrush(Color.FromArgb(255, (byte)150, (byte)200, (byte)20));
            }
            return new SolidColorBrush(Color.FromArgb(255, (byte)220, (byte)0, (byte)20));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
