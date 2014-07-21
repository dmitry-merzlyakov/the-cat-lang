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
    public sealed class ItemChangedEvent : BaseEvent<FileSystemItemDescriptor>
    {
        public ItemChangedEvent(FileSystemItemDescriptor descriptor, ItemModificationType modificationType) : base (descriptor)
        {
            ModificationType = modificationType;
        }

        public ItemModificationType ModificationType { get; private set; }
    }

    public enum ItemModificationType
    {
        Created,
        Updated,
        Deleted
    }
}
