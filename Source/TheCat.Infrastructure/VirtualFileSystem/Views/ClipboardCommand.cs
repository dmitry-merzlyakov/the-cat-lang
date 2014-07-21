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
using TheCat.Infrastructure.Events;
using TheCat.Infrastructure.VirtualFileSystem.Events;

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public sealed class ClipboardCommand : Command
    {
        public ClipboardCommand(Action pasteAction)
            : base(pasteAction)
        {
            UpdateCanExecute();
            Locator.Get<EventManager>().RegisterSubscription<ClipboardChangedEvent>(ClipboardChangedHandler);
        }

        private void ClipboardChangedHandler(ClipboardChangedEvent evnt)
        {
            UpdateCanExecute();
        }

        private void UpdateCanExecute()
        {
            CanExecute = FileSystemItemClipboard.Current.Item != null;
        }
    }
}
