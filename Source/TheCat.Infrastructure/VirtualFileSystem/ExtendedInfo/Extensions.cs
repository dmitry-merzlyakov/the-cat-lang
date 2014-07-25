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

namespace TheCat.Infrastructure.VirtualFileSystem.ExtendedInfo
{
    public static class Extensions
    {
        public static string GetDescriptor(this ExtendedInfoItem extendedInfoItem)
        {
            return extendedInfoItem == null ? String.Empty : extendedInfoItem.Description;
        }

        public static ExtendedInfoItem GetExtendedInfoItem(this FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            return new ExtendedInfoItem()
            {
                Name = fileSystemItemDescriptor.Name,
                Description = fileSystemItemDescriptor.Description
            };
        }
    }
}
