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
    public class CreateFileViewModel : BaseFileDescriptorViewModel
    {
        public CreateFileViewModel(IVirtualFileSystemRepository repository, string currentFolderName)
            : base(repository)
        {
            CurrentFolderName = currentFolderName;
            Name = Repository.GenerateUniqueName(CurrentFolderName, FileNameTemplate) + DefaultExtension;
        }

        protected override string GetTitle()
        {
            return "new file";
        }

        protected override bool CommitChanges(ref string messages)
        {
            FileSystemItemDescriptor descriptor = new FileSystemItemDescriptor()
            {
                FullName = System.IO.Path.Combine(CurrentFolderName, Name),
                Name = Name,
                IsFolder = false
            };

            FileSystemResult result = Repository.Create(descriptor);

            if (result.IsOK)
                MessageBox.Show("File is created. Please tap on the new file to edit the content.");

            messages = result.ErrorMessage;
            return result.IsOK;
        }

        private readonly string CurrentFolderName;
        private const string FileNameTemplate = "NewFile-";
        private const string DefaultExtension = ".cat";
    }
}
