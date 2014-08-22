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

namespace TheCat.Infrastructure.Common
{
    public sealed class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            OpenConsole = new Command(() =>  Locator.Get<INavigationManager>().Navigate(StringKeys.RunConsole));
            OpenSessions = new Command(() => Locator.Get<INavigationManager>().Navigate(StringKeys.ViewSessions));
            OpenFiles = new Command(() => Locator.Get<INavigationManager>().Navigate(StringKeys.ViewFiles));
            OpenSettings = new Command(() => Locator.Get<INavigationManager>().Navigate(StringKeys.ViewSettings));
            ShowHelp = new Command(() => MessageBox.Show("Coming soon..."));

            GlobalInitialization();
        }

        public void GlobalInitialization()
        {
            // Initialize File System (first run)
            IEnvironmentManager environmentManager = Locator.Get<IEnvironmentManager>();
            if (!environmentManager.IsEnvironmentInitialized)
                environmentManager.InitializeEnvironment();
        }

        public ICommand OpenConsole { get; private set; }
        public ICommand OpenSessions { get; private set; }
        public ICommand OpenFiles { get; private set; }
        public ICommand OpenSettings { get; private set; }
        public ICommand ShowHelp { get; private set; }
    }
}
