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
    public class ChangeFileViewModel : BaseFileDescriptorViewModel
    {
        public ChangeFileViewModel(IVirtualFileSystemRepository repository, string fileName)
            : base(repository)
        {
            FileDescriptor = Repository.GetFile(fileName);
            Name = FileDescriptor.Name;
            Description = FileDescriptor.Description;
        }

        protected override string GetTitle()
        {
            return "rename file";
        }

        protected override bool CommitChanges(ref string messages)
        {
            FileDescriptor.Name = Name;
            FileDescriptor.Description = Description;

            FileSystemResult result = Repository.Update(FileDescriptor);

            messages = result.ErrorMessage;
            return result.IsOK;
        }

        private FileSystemItemDescriptor FileDescriptor { get; set; }

    }
}
