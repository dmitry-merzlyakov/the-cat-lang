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
using System.IO;

namespace Cat.Abstract
{
    public interface IFileSystemProvider
    {
        StreamReader GetStreamReader(string fileName);
        StreamWriter GetStreamWriter(string fileName);
        bool DirectoryExists(string folderName);
        void CreateDirectory(string folderName);
        bool FileExists(string fileName);

        // File.OpenRead(string filename)
        Stream OpenStream(string fileName);

        // File.Create(string filename)
        Stream CreateStream(string fileName);
    }
}
