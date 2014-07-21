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

namespace TheCat.Infrastructure.VirtualFileSystem
{
    public enum FileSystemErrorCode
    {
        OK,
        Unknown
    }

    public class FileSystemResult
    {
        public static readonly FileSystemResult OK = new FileSystemResult(FileSystemErrorCode.OK);

        public FileSystemResult(FileSystemErrorCode errorCode, string errorMessage = null)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public bool IsOK
        {
            get { return ErrorCode == FileSystemErrorCode.OK; }
        }

        public FileSystemErrorCode ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
