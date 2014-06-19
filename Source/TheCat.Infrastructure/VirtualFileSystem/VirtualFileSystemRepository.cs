using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;

namespace TheCat.Infrastructure.VirtualFileSystem
{
    public class VirtualFileSystemRepository : IVirtualFileSystemRepository
    {
        public VirtualFileSystemRepository(IExtendedFileSystemProvider provider)
        {
            Provider = provider;
            Cache = new FileSystemCache()
            {
                FileFactory = FileFactory,
                FolderFactory = FolderFactory
            };
        }

        public IExtendedFileSystemProvider Provider
        {
            get { return _Provider; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Provider = value;
            }
        }

        public IEnumerable<FileSystemItemDescriptor> GetFolderContent(string fullFolderName = null)
        {
            if (String.IsNullOrWhiteSpace(fullFolderName))
                fullFolderName = @"\";

            if (!Provider.DirectoryExists(fullFolderName))
                throw new ArgumentException(String.Format("Folder '{0}' does not exist", fullFolderName));

            string pattern = System.IO.Path.Combine(fullFolderName, "*.*");
            List<FileSystemItemDescriptor> items = new List<FileSystemItemDescriptor>();
            items.AddRange(Provider.GetDirectoryNames(pattern).Select(n => Cache.GetFolder(System.IO.Path.Combine(fullFolderName, n))));
            items.AddRange(Provider.GetFileNames(pattern).Select(n => Cache.GetFile(System.IO.Path.Combine(fullFolderName, n))));
            return items.OrderBy(d => d.OrderKey);
        }

        public FileSystemItemContent GetFileContent(string fullFileName)
        {
            using (StreamReader sr = Provider.GetStreamReader(fullFileName))
            {
                return new FileSystemItemContent()
                {
                    FullName = fullFileName,
                    Content = sr.ReadToEnd()
                };
            }
        }

        public FileSystemItemDescriptor GetFolder(string fullFolderName)
        {
            return Cache.GetFolder(fullFolderName);
        }

        public FileSystemItemDescriptor GetFile(string fullFolderName)
        {
            return Cache.GetFile(fullFolderName);
        }

        private FileSystemItemDescriptor FolderFactory(string name)
        {
            return CreateFileSystemItemDescriptor(name, true);
        }

        private FileSystemItemDescriptor FileFactory(string name)
        {
            return CreateFileSystemItemDescriptor(name, false);
        }

        private FileSystemItemDescriptor CreateFileSystemItemDescriptor(string name, bool isFolder)
        {
            string shortName = System.IO.Path.GetFileName(name);
            return new FileSystemItemDescriptor()
            {
                FullName = name,
                Name = shortName,
                Description = String.Empty,
                IsFolder = isFolder,
                CreatedDate = Provider.GetCreationTime(name),
                LastUpdateDate = Provider.GetLastWriteTime(name)
            };
        }

        private IExtendedFileSystemProvider _Provider;
        private readonly FileSystemCache Cache;
    }
}
