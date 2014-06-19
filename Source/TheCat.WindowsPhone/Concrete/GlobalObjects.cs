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
using TheCat.Infrastructure.VirtualFileSystem;
using TheCat.Infrastructure;

namespace TheCat.WindowsPhone.Concrete
{
    // TODO - replace with IoC
    public static class GlobalObjects
    {
        public static void EnsureInitialized()
        {
            if (!IsInitialized)
            {
                ExtendedFileSystemProvider = new IsolatedStorageFileSystem();
                VirtualFileSystemRepository = new VirtualFileSystemRepository(ExtendedFileSystemProvider);
                NavigationManager.Configure(new WindowsPhoneNavigationManager());

                IsInitialized = true;
            }
        }

        public static IExtendedFileSystemProvider ExtendedFileSystemProvider { get; set; }
        public static IVirtualFileSystemRepository VirtualFileSystemRepository { get; set; }

        private static bool IsInitialized { get; set; }
    }
}
