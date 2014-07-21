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
    public class FileDescriptorImageNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FileSystemItemDescriptor descriptor = (FileSystemItemDescriptor)value;
            if (descriptor == null)
                throw new ArgumentNullException("value");

            string imageName;

            // TODO - add Root folder
            if (descriptor.IsFolder)
                imageName = "appbar.folder";
            else
                imageName = "appbar.page.small";

            // Uri is not required. See here: http://mikaelkoskinen.net/winrt-xaml-image-binding-compared-to-windows-phone-7/
            //var uri = new Uri("pack://application:,,,/XConsole;component/Images/" + imageName + ".png"); 
            //return new System.Windows.Media.Imaging.BitmapImage(uri);
            return String.Format(@"Images\{0}.png", imageName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack");
        }
    }
}
