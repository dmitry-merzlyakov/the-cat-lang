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
using TheCat.Infrastructure.Sessions;

namespace TheCat.Infrastructure.Common
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            EditDefaultConsole = new Command((d) => Locator.Get<INavigationManager>().Navigate(StringKeys.EditSession, CompositeParams.Create(StringKeys.SessionName, SessionDefinitionRepository.SessionDefinitionDefaultID)));
        }

        public ICommand EditDefaultConsole { get; private set; }
    }
}
