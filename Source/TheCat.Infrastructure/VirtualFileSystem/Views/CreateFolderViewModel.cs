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
using TheCat.Infrastructure.VirtualFileSystem;

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public class CreateFolderViewModel : BaseFileDescriptorViewModel
    {
        public CreateFolderViewModel(IVirtualFileSystemRepository repository, string currentFolderName)
            : base(repository)
        {
            CurrentFolderName = currentFolderName;
            Name = Repository.GenerateUniqueName(CurrentFolderName, FolderNameTemplate);
        }

        protected override string GetTitle()
        {
            return "new folder";
        }

        protected override bool CommitChanges(ref string messages)
        {
            FileSystemItemDescriptor descriptor = new FileSystemItemDescriptor()
            {
                FullName = System.IO.Path.Combine(CurrentFolderName, Name),
                Name = Name,
                IsFolder = true
            };

            FileSystemResult result = Repository.Create(descriptor);

            messages = result.ErrorMessage;
            return result.IsOK;
        }

        private readonly string CurrentFolderName;
        private const string FolderNameTemplate = "NewFolder-";
    }
}
