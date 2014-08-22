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
using System.ComponentModel;

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public class EditFileViewModel : BaseViewModel
    {
        public EditFileViewModel(IVirtualFileSystemRepository repository, string fileName)
        {
            Repository = repository;
            FileName = fileName;
            Initialize();
        }

        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
                OnPropertyChanged("FullName");
            }
        }

        public string FileName
        {
            get { return _FileName; }
            set
            {
                _FileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string ShortFileName
        {
            get { return _ShortFileName; }
            set
            {
                _ShortFileName = value;
                OnPropertyChanged("ShortFileName");
            }
        }

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        public DateTimeOffset Created
        {
            get { return _Created; }
            set
            {
                _Created = value;
                OnPropertyChanged("Created");
            }
        }

        public DateTimeOffset Updated
        {
            get { return _Updated; }
            set
            {
                _Updated = value;
                OnPropertyChanged("Updated");
            }
        }

        public string Content
        {
            get { return _Content; }
            set
            {
                IsContentChanged = _Content != value;
                _Content = value;
                OnPropertyChanged("Content");
            }
        }

        public bool IsContentChanged { get; set; }

        public void ValidateAndSave()
        {
            if (IsContentChanged)
            {
                FileSystemItemContent.Content = Content;
                Repository.UpdateContent(FileSystemItemContent);
                IsContentChanged = false;
            }
        }

        public void SaveAndRun()
        {
            ValidateAndSave();
            Locator.Get<INavigationManager>().Navigate(StringKeys.RunConsoleWithFile, CompositeParams.Create(StringKeys.FileName, FileSystemItemDescriptor.FullName));
        }

        private FileSystemItemDescriptor FileSystemItemDescriptor { get; set; }
        private FileSystemItemContent FileSystemItemContent { get; set; }

        private IVirtualFileSystemRepository Repository
        {
            get { return _Repository; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Repository = value;
            }
        }

        private void Initialize()
        {
            // TODO - new items...
            FileSystemItemDescriptor = Repository.GetFile(FileName);
            FileSystemItemContent = Repository.GetFileContent(FileName);

            ShortFileName = FileSystemItemDescriptor.Name;
            FullName = FileSystemItemDescriptor.FullName;
            Description = FileSystemItemDescriptor.Description;
            Created = FileSystemItemDescriptor.CreatedDate;
            Updated = FileSystemItemDescriptor.LastUpdateDate;
            Content = FileSystemItemContent.Content;

            IsContentChanged = false;
        }

        private IVirtualFileSystemRepository _Repository;
        private string _FullName;
        private string _FileName;
        private string _ShortFileName;
        private string _Description;
        private DateTimeOffset _Created;
        private DateTimeOffset _Updated;
        private string _Content;
    }
}
