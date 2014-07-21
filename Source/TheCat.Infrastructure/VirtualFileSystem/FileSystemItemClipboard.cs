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

namespace TheCat.Infrastructure.VirtualFileSystem
{
    public enum ClipboardActionEnum
    {
        None, Copy, Cut
    }

    public class FileSystemItemClipboard
    {
        public static FileSystemItemClipboard Current
        {
            get { return Lazy<FileSystemItemClipboard>.Value; }
        }

        public static bool IsEmpty
        {
            get { return !Lazy<FileSystemItemClipboard>.HasValue || Lazy<FileSystemItemClipboard>.Value.Item != null; }
        }

        public FileSystemItemDescriptor Item { get; private set; }
        public ClipboardActionEnum ClipboardAction { get; private set; }

        public void SetItem(FileSystemItemDescriptor item, ClipboardActionEnum clipboardAction)
        {
            FileSystemItemDescriptor previousItem = Item;
            Item = item;

            if (clipboardAction == ClipboardActionEnum.None)
                throw new ArgumentException("clipboardAction");

            ClipboardAction = clipboardAction;
            Locator.Get<EventManager>().PublishEvent(new ClipboardChangedEvent(item, previousItem));
        }

        public void Clear()
        {
            FileSystemItemDescriptor previousItem = Item;
            Item = null;
            ClipboardAction = ClipboardActionEnum.None;
            Locator.Get<EventManager>().PublishEvent(new ClipboardChangedEvent(null, previousItem));
        }
    }
}
