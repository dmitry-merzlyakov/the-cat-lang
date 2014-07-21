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

namespace TheCat.Infrastructure.VirtualFileSystem.Events
{
    public sealed class ClipboardChangedEvent : IEvent
    {
        public ClipboardChangedEvent(FileSystemItemDescriptor actualDescriptor, FileSystemItemDescriptor previousDescriptor)
        {
            ActualDescriptor = actualDescriptor;
            PreviousDescriptor = previousDescriptor;
        }

        public FileSystemItemDescriptor ActualDescriptor { get; private set; }
        public FileSystemItemDescriptor PreviousDescriptor { get; private set; }

        public bool IsClipboardEmpty
        {
            get { return ActualDescriptor == null; }
        }
    }
}
