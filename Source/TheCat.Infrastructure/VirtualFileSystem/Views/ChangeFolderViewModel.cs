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

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public class ChangeFolderViewModel : BaseFileDescriptorViewModel
    {
        public ChangeFolderViewModel(IVirtualFileSystemRepository repository, string folderName)
            : base(repository)
        {
            FolderDescriptor = Repository.GetFolder(folderName);
            Name = FolderDescriptor.Name;
            Description = FolderDescriptor.Description;
        }

        protected override string GetTitle()
        {
            return "rename folder";
        }

        protected override bool CommitChanges(ref string messages)
        {
            FolderDescriptor.Name = Name;
            FolderDescriptor.Description = Description;

            FileSystemResult result = Repository.Update(FolderDescriptor);

            messages = result.ErrorMessage;
            return result.IsOK;
        }

        private FileSystemItemDescriptor FolderDescriptor { get; set; }
    }
}
