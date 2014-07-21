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
using TheCat.Infrastructure;
using TheCat.Infrastructure.VirtualFileSystem;

namespace TheCat.WindowsPhone.Concrete
{
    public static class LocatorConfigurer
    {
        public static void Configure()
        {
            Locator.AddService<IExtendedFileSystemProvider>((p) => new IsolatedStorageFileSystem());
            Locator.AddService<INavigationManager>((p) => new WindowsPhoneNavigationManager());
        }
    }
}
