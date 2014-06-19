﻿using System;
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

namespace TheCat.Infrastructure.VirtualFileSystem
{
    /// <summary>
    /// Interface that presents extended file system functions (that are not required for the core)
    /// </summary>
    public interface IExtendedFileSystemProvider : IFileSystemProvider
    {
        string[] GetDirectoryNames(string folderName);
        string[] GetFileNames(string folderName);
        DateTimeOffset GetCreationTime(string path);
        DateTimeOffset GetLastWriteTime(string path);
    }
}
