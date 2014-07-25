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
using TheCat.Infrastructure.Events;
using TheCat.Infrastructure.VirtualFileSystem.Events;
using TheCat.Infrastructure.VirtualFileSystem.ExtendedInfo;

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

        public FileSystemResult UpdateContent(FileSystemItemContent fileSystemItemContent)
        {
            if (fileSystemItemContent == null)
                throw new ArgumentNullException("fileSystemItemContent");

            try
            {
                using (StreamWriter sw = Provider.GetStreamWriter(fileSystemItemContent.FullName))
                {
                    sw.Write(fileSystemItemContent.Content);
                }
            }
            catch (Exception ex)
            {
                return new FileSystemResult(FileSystemErrorCode.Unknown, ex.Message);
            }

            return FileSystemResult.OK;
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
            ExtendedInfoItem extendedInfoItem = ExtendedInfoManager.Current.Get(name);
            return new FileSystemItemDescriptor()
            {
                FullName = name,
                Name = name.GetFileName(),
                Description = extendedInfoItem.GetDescriptor(),
                IsFolder = isFolder,
                CreatedDate = Provider.GetCreationTime(name),
                LastUpdateDate = Provider.GetLastWriteTime(name)
            };
        }

        private IExtendedFileSystemProvider _Provider;
        private readonly FileSystemCache Cache;


        public FileSystemResult Create(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");

            try
            {
                if (fileSystemItemDescriptor.IsFolder)
                    Provider.CreateDirectory(fileSystemItemDescriptor.FullName);
                else
                    using (Provider.CreateStream(fileSystemItemDescriptor.FullName)) { };

                ExtendedInfoManager.Current.SaveOrUpdate(fileSystemItemDescriptor.ParentFolderName, fileSystemItemDescriptor.GetExtendedInfoItem());
            }
            catch (Exception ex)
            {
                return new FileSystemResult(FileSystemErrorCode.Unknown, ex.Message);
            }

            Locator.Get<EventManager>().PublishEvent(new ItemChangedEvent(fileSystemItemDescriptor, ItemModificationType.Created));
            return FileSystemResult.OK;
        }

        public FileSystemResult Update(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");

            string fullTargetName = System.IO.Path.Combine(fileSystemItemDescriptor.ParentFolderName, fileSystemItemDescriptor.Name);

            try
            {
                if (fileSystemItemDescriptor.IsFolder)
                {
                    if (fileSystemItemDescriptor.IsRootFolder)
                        throw new InvalidOperationException("Cannot rename the root folder");

                    Provider.MoveDirectory(fileSystemItemDescriptor.FullName, fullTargetName);
                }
                else
                {
                    Provider.MoveFile(fileSystemItemDescriptor.FullName, fullTargetName);
                }

                ExtendedInfoManager.Current.SaveOrUpdate(fileSystemItemDescriptor.ParentFolderName, fileSystemItemDescriptor.GetExtendedInfoItem());
            }
            catch (Exception ex)
            {
                return new FileSystemResult(FileSystemErrorCode.Unknown, ex.Message);
            }

            Locator.Get<EventManager>().PublishEvent(new ItemChangedEvent(fileSystemItemDescriptor, ItemModificationType.Updated));
            return FileSystemResult.OK;
        }

        public FileSystemResult Delete(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");

            try
            {
                if (fileSystemItemDescriptor.IsFolder)
                {
                    if (fileSystemItemDescriptor.IsRootFolder)
                        throw new InvalidOperationException("Cannot delete the root folder");

                    DeleteDirectory(fileSystemItemDescriptor.FullName);
                }
                else
                {
                    Provider.DeleteFile(fileSystemItemDescriptor.FullName);
                }

                ExtendedInfoManager.Current.Delete(fileSystemItemDescriptor.FullName);
            }
            catch (Exception ex)
            {
                return new FileSystemResult(FileSystemErrorCode.Unknown, ex.Message);
            }

            Locator.Get<EventManager>().PublishEvent(new ItemChangedEvent(fileSystemItemDescriptor, ItemModificationType.Deleted));
            return FileSystemResult.OK;
        }

        public FileSystemResult Copy(FileSystemItemDescriptor fileSystemItemDescriptor, FileSystemItemDescriptor targetFolder, string targetName)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");
            if (targetFolder == null)
                throw new ArgumentNullException("targetFolder");

            string fullTargetName = System.IO.Path.Combine(targetFolder.FullName, String.IsNullOrWhiteSpace(targetName) ? fileSystemItemDescriptor.Name : targetName);

            try
            {
                if (fileSystemItemDescriptor.IsFolder)
                    CopyDirectory(fileSystemItemDescriptor.FullName, fullTargetName);
                else
                    Provider.CopyFile(fileSystemItemDescriptor.FullName, fullTargetName);

                ExtendedInfoManager.Current.SaveOrUpdate(fileSystemItemDescriptor.FullName, fileSystemItemDescriptor.GetExtendedInfoItem());
            }
            catch (Exception ex)
            {
                return new FileSystemResult(FileSystemErrorCode.Unknown, ex.Message);
            }

            FileSystemItemDescriptor copiedDescriptor = GetFolderContent(targetFolder.FullName).First(d => d.FullName == fullTargetName);
            Locator.Get<EventManager>().PublishEvent(new ItemChangedEvent(copiedDescriptor, ItemModificationType.Created));

            return FileSystemResult.OK;
        }

        public FileSystemResult Move(FileSystemItemDescriptor fileSystemItemDescriptor, FileSystemItemDescriptor targetFolder, string targetName)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");
            if (targetFolder == null)
                throw new ArgumentNullException("targetFolder");

            string fullTargetName = System.IO.Path.Combine(targetFolder.FullName, String.IsNullOrWhiteSpace(targetName) ? fileSystemItemDescriptor.Name : targetName);

            try
            {
                if (fileSystemItemDescriptor.IsFolder)
                    Provider.MoveDirectory(fileSystemItemDescriptor.FullName, fullTargetName);
                else
                    Provider.MoveFile(fileSystemItemDescriptor.FullName, fullTargetName);

                ExtendedInfoManager.Current.ClearCache();
            }
            catch (Exception ex)
            {
                return new FileSystemResult(FileSystemErrorCode.Unknown, ex.Message);
            }

            Locator.Get<EventManager>().PublishEvent(new ItemChangedEvent(fileSystemItemDescriptor, ItemModificationType.Deleted));
            FileSystemItemDescriptor movedDescriptor = GetFolderContent(targetFolder.FullName).First(d => d.FullName == fullTargetName);
            Locator.Get<EventManager>().PublishEvent(new ItemChangedEvent(movedDescriptor, ItemModificationType.Created));

            return FileSystemResult.OK;
        }

        /// <summary>
        /// Deletes a directory with all its content
        /// </summary>
        private void DeleteDirectory(string directoryName)
        {
            string[] directoryNames = Provider.GetDirectoryNames(directoryName + @"\*.*");
            foreach (string childDirectoryName in directoryNames)
                DeleteDirectory(System.IO.Path.Combine(directoryName, childDirectoryName));

            string[] fileNames = Provider.GetFileNames(directoryName + @"\*.*");
            foreach (string fileName in fileNames)
                Provider.DeleteFile(System.IO.Path.Combine(directoryName, fileName));

            Provider.DeleteDirectory(directoryName);
        }

        /// <summary>
        /// Copies a directory with all its content
        /// </summary>
        private void CopyDirectory(string sourceDirectoryName, string targetDirectoryName)
        {
            Provider.CreateDirectory(targetDirectoryName);

            string[] directoryNames = Provider.GetDirectoryNames(sourceDirectoryName + @"\*.*");
            foreach (string childDirectoryName in directoryNames)
                CopyDirectory(System.IO.Path.Combine(sourceDirectoryName, childDirectoryName), System.IO.Path.Combine(targetDirectoryName, childDirectoryName));

            string[] fileNames = Provider.GetFileNames(sourceDirectoryName + @"\*.*");
            foreach (string fileName in fileNames)
                Provider.CopyFile(System.IO.Path.Combine(sourceDirectoryName, fileName), System.IO.Path.Combine(targetDirectoryName, fileName));
        }
    }
}
