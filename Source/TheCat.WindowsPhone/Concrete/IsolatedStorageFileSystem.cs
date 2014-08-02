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
using Cat.Abstract;
using System.IO;
using System.IO.IsolatedStorage;
using TheCat.Infrastructure.VirtualFileSystem;

namespace TheCat.WindowsPhone.Concrete
{
    public class IsolatedStorageFileSystem : IExtendedFileSystemProvider
    {
        public IsolatedStorageFileSystem()
        {
            // TODO - IDisposable
            IsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public IsolatedStorageFile IsolatedStorage { get; private set; }

        public StreamReader GetStreamReader(string fileName)
        {
            return new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, IsolatedStorage));
        }

        public StreamWriter GetStreamWriter(string fileName)
        {
            return new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, IsolatedStorage));
        }

        public bool DirectoryExists(string folderName)
        {
            return IsolatedStorage.DirectoryExists(folderName);
        }

        public void CreateDirectory(string folderName)
        {
            IsolatedStorage.CreateDirectory(folderName);
        }

        public bool FileExists(string fileName)
        {
            return IsolatedStorage.FileExists(fileName);
        }


        public Stream OpenStream(string fileName)
        {
            return IsolatedStorage.OpenFile(fileName, FileMode.Open);
        }

        public Stream CreateStream(string fileName)
        {
            return IsolatedStorage.OpenFile(fileName, FileMode.Create);
        }

        public string[] GetDirectoryNames(string folderName)
        {
            return IsolatedStorage.GetDirectoryNames(folderName);
        }

        public string[] GetFileNames(string fileName)
        {
            return IsolatedStorage.GetFileNames(fileName);
        }

        public DateTimeOffset GetCreationTime(string path)
        {
            return IsolatedStorage.GetCreationTime(path);
        }

        public DateTimeOffset GetLastWriteTime(string path)
        {
            return IsolatedStorage.GetLastWriteTime(path);
        }


        public void DeleteDirectory(string dir)
        {
            IsolatedStorage.DeleteDirectory(dir);
        }

        public void DeleteFile(string file)
        {
            IsolatedStorage.DeleteFile(file);
        }

        public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
            IsolatedStorage.MoveDirectory(sourceDirectoryName, destinationDirectoryName);
        }

        public void MoveFile(string sourceFileName, string destinationFileName)
        {
            IsolatedStorage.MoveFile(sourceFileName, destinationFileName);
        }

        public void CopyFile(string sourceFileName, string destinationFileName)
        {
            IsolatedStorage.CopyFile(sourceFileName, destinationFileName);
        }

        public object GetRegistryValue(string registryKey)
        {
            if (String.IsNullOrWhiteSpace(registryKey))
                throw new ArgumentException("registryKey");

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            return settings.Contains(registryKey) ? settings[registryKey] : null;
        }

        public void SetRegistryValue(string registryKey, object registryValue)
        {
            if (String.IsNullOrWhiteSpace(registryKey))
                throw new ArgumentException("registryKey");

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(registryKey))
                settings[registryKey] = registryValue;
            else
                settings.Add(registryKey, registryValue);
        }

    }
}
