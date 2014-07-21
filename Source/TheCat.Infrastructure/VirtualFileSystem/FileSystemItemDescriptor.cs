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
    public class FileSystemItemDescriptor
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFolder { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset LastUpdateDate { get; set; }

        // TODO - subject for optimization
        public string OrderKey
        {
            get { return IsFolder.ToString() + Name.ToLower(); }
        }

        public bool IsRootFolder
        {
            get { return string.IsNullOrWhiteSpace(FullName) || FullName == @"\"; }
        }

        public string ParentFolderName
        {
            get 
            { 
                string parentFolderName =  FullName.Substring(0, FullName.LastIndexOf(@"\"));
                return String.IsNullOrWhiteSpace(parentFolderName) ? @"\" : parentFolderName;
            }
        }
    }
}
