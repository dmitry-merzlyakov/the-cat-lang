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
using System.Collections.Generic;

namespace TheCat.Infrastructure.VirtualFileSystem
{
    public interface IVirtualFileSystemRepository
    {
        IEnumerable<FileSystemItemDescriptor> GetFolderContent(string fullFolderName = null);
        FileSystemItemContent GetFileContent(string fullFileName);
        FileSystemItemDescriptor GetFolder(string fullFolderName);
        FileSystemItemDescriptor GetFile(string fullFileName);
        void UpdateContent(FileSystemItemContent fileSystemItemContent);
    }
}
