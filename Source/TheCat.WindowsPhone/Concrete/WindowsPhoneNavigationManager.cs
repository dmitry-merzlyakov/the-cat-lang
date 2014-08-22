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
using System.Collections.Generic;

namespace TheCat.WindowsPhone.Concrete
{
    public class WindowsPhoneNavigationManager : INavigationManager
    {
        public WindowsPhoneNavigationManager()
        {
            PageMapping.Add(StringKeys.CreateFile, "/EditFileDescPage.xaml");
            PageMapping.Add(StringKeys.CreateFolder, "/EditFileDescPage.xaml");
            PageMapping.Add(StringKeys.EditFile, "/EditFilePage.xaml");
            PageMapping.Add(StringKeys.ModifyFile, "/EditFileDescPage.xaml");
            PageMapping.Add(StringKeys.ModifyFolder, "/EditFileDescPage.xaml");
            PageMapping.Add(StringKeys.ViewFiles, "/FilesPage.xaml");
            PageMapping.Add(StringKeys.ViewSessions, "/SessionsPage.xaml");
            PageMapping.Add(StringKeys.CreateSession, "/EditSessionPage.xaml");
            PageMapping.Add(StringKeys.EditSession, "/EditSessionPage.xaml");
            PageMapping.Add(StringKeys.RunConsole, "/ConsolePage.xaml");
            PageMapping.Add(StringKeys.RunConsoleWithSession, "/ConsolePage.xaml");
            PageMapping.Add(StringKeys.RunConsoleWithFile, "/ConsolePage.xaml");
            PageMapping.Add(StringKeys.ViewSettings, "/EditSettingsPage.xaml");
        }

        public PhoneApplicationPage CurrentPage
        {
            get { return ((App)Application.Current).RootFrame.Content as PhoneApplicationPage; }
        }

        public void Navigate(string target, CompositeParams parms = null)
        {
            if (String.IsNullOrWhiteSpace(target))
                throw new ArgumentOutOfRangeException("target");

            string pageName;
            if (!PageMapping.TryGetValue(target, out pageName))
                throw new ArgumentNullException(String.Format("Navigation mapping is not specified for '{0}'", target));

            // TODO - Query string? State?
            // Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EditFilePage.xaml"] = asdasd;
            TempTarget = target;
            TempParams = parms;

            string uniqQueryString = String.Format("?t={0};h={1}", String.IsNullOrWhiteSpace(target) ? "none" : target, parms == null ? 0 : parms.GetHashCode());
            CurrentPage.NavigationService.Navigate(new Uri(pageName + uniqQueryString, UriKind.Relative));
        }

        /*
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
            if (target == "CreateFolder")
            {
                if (parameters.Length != 0)
                    throw new InvalidOperationException("Wrong number of parameters");
                Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EditFileDescPage.xaml"] = "CreateFolder";
                
                PhoneApplicationPage currentPage = ((App)Application.Current).RootFrame.Content as PhoneApplicationPage;
                currentPage.NavigationService.Navigate(new Uri("/EditFileDescPage.xaml", UriKind.Relative));
            }
        }
         */

        public void GoBack()
        {
            //PhoneApplicationPage currentPage = ((App)Application.Current).RootFrame.Content as PhoneApplicationPage;
            CurrentPage.NavigationService.GoBack();
        }

        public string PopTarget()
        {
            string tempTarget = TempTarget;
            TempTarget = null;
            return tempTarget;
        }

        public CompositeParams PopParams()
        {
            CompositeParams tempParams = TempParams;
            TempParams = null;
            return tempParams;
        }

        private string TempTarget { get; set; }
        private CompositeParams TempParams { get; set; }

        private readonly IDictionary<string, string> PageMapping = new Dictionary<string, string>();
    }
}
