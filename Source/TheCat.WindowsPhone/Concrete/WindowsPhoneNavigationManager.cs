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
using TheCat.Infrastructure.VirtualFileSystem.Views;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace TheCat.WindowsPhone.Concrete
{
    public class WindowsPhoneNavigationManager : INavigationManager
    {
        public void Navigate(string target, params object[] parameters)
        {
            // TODO - create "normal" implementation
            if (target == "EditFile")
            {
                if (parameters.Length != 1)
                    throw new InvalidOperationException("Wrong number of parameters");
                Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EditFilePage.xaml"] = parameters[0];

                PhoneApplicationPage currentPage = ((App)Application.Current).RootFrame.Content as PhoneApplicationPage;
                currentPage.NavigationService.Navigate(new Uri("/EditFilePage.xaml", UriKind.Relative));
            }
        }
    }
}
