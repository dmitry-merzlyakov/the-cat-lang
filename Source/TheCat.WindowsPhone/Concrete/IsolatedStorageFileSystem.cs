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

namespace TheCat.WindowsPhone.Concrete
{
    public class IsolatedStorageFileSystem : IFileSystemProvider
    {
        public IsolatedStorageFileSystem()
        {
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
            throw new NotImplementedException();
        }

        public Stream CreateStream(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
