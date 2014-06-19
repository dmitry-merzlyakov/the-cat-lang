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
    public sealed class FileSystemCache
    {
        // TODO - optimize
        public Func<string, FileSystemItemDescriptor> FolderFactory { get; set; }
        public Func<string, FileSystemItemDescriptor> FileFactory { get; set; }

        public FileSystemItemDescriptor GetFolder(string folderName)
        {
            // TODO - check last acces

            // TODO - !
            if (String.IsNullOrWhiteSpace(folderName))
                folderName = @"\";

            FileSystemItemDescriptor descriptor;
            if (!Items.TryGetValue(folderName, out descriptor))
            {
                descriptor = FolderFactory(folderName);
                Items[folderName] = descriptor;
            }
            return descriptor;
        }

        public FileSystemItemDescriptor GetFile(string fileName)
        {
            // TODO - check last acces

            FileSystemItemDescriptor descriptor;
            if (!Items.TryGetValue(fileName, out descriptor))
            {
                descriptor = FileFactory(fileName);
                Items[fileName] = descriptor;
            }
            return descriptor;
        }

        private readonly IDictionary<string, FileSystemItemDescriptor> Items = new Dictionary<string, FileSystemItemDescriptor>();
    }
}
