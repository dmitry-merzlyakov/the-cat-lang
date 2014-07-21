using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using TheCat.Infrastructure.VirtualFileSystem;

namespace TheCat.WindowsPhone.Controls
{
    public class FileDescriptorBackgroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FileSystemItemDescriptor descriptor = (FileSystemItemDescriptor)value;
            if (descriptor == null)
                throw new ArgumentNullException("value");

            Brush backgroundBrush = Application.Current.Resources["PhoneAccentBrush"] as Brush;

            if (FileSystemItemClipboard.Current.Item == descriptor)
            {
                if (FileSystemItemClipboard.Current.ClipboardAction == ClipboardActionEnum.Copy)
                    backgroundBrush = Application.Current.Resources["PhoneBorderBrush"] as Brush;
                else
                    backgroundBrush = Application.Current.Resources["PhoneSemitransparentBrush"] as Brush;
            }

            return backgroundBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack");
        }
    }
}
