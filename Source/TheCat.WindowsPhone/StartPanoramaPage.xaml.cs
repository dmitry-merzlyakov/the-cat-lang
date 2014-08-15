using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TheCat.Infrastructure;

namespace TheCat.WindowsPhone
{
    public partial class StartPanoramaPage : PhoneApplicationPage
    {
        public StartPanoramaPage()
        {
            InitializeComponent();

            // TODO - should be moved to the main model
            IEnvironmentManager environmentManager = Locator.Get<IEnvironmentManager>();
            if (!environmentManager.IsEnvironmentInitialized)
                environmentManager.InitializeEnvironment();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Locator.Get<INavigationManager>().Navigate(StringKeys.ViewFiles);
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Locator.Get<INavigationManager>().Navigate(StringKeys.RunConsole);
            //NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Locator.Get<INavigationManager>().Navigate(StringKeys.ViewSessions);
        }
    }
}