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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public class FilesViewModel : INotifyPropertyChanged
    {
        public FilesViewModel(IVirtualFileSystemRepository repository)
        {
            Repository = repository;
            ChangeCurrentFolder();
        }

        public string CurrentFolderName
        {
            get { return _CurrentFolderName; }
            set
            {
                _CurrentFolderName = value;
                OnPropertyChanged("CurrentFolderName");
            }
        }

        public ObservableCollection<FileSystemItemDescriptor> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                OnPropertyChanged("Items");
            }
        }

        public FileSystemItemDescriptor SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");

                // Handle selected item
                if (SelectedItem != null)
                {
                    if (SelectedItem.IsFolder)
                        ChangeCurrentFolder(SelectedItem.FullName);
                    else
                        OpenFile(SelectedItem.FullName);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private FileSystemItemDescriptor CurrentFolder { get; set; }

        private void OnPropertyChanged(string propertyChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
        }

        private void ChangeCurrentFolder(string fullFolderName = null)
        {
            // Set the current folder
            CurrentFolder = Repository.GetFolder(fullFolderName);
            CurrentFolderName = CurrentFolder.Name;
            // Update items
            Items.Clear();
            if (!CurrentFolder.IsRootFolder)
                Items.Add(CreateParentFolder(CurrentFolder.ParentFolderName));
            foreach (FileSystemItemDescriptor item in Repository.GetFolderContent(fullFolderName))
                Items.Add(item);
        }

        private void OpenFile(string fileName)
        {
            NavigationManager.Current.Navigate("EditFile", fileName);
        }

        private FileSystemItemDescriptor CreateParentFolder(string folderName)
        {
            FileSystemItemDescriptor parentFolder = Repository.GetFolder(folderName);

            return new FileSystemItemDescriptor()
            {
                Name = ParentFolderName,
                Description = ParentFolderDesc,
                FullName = folderName,
                IsFolder = true,
                CreatedDate = parentFolder.CreatedDate,
                LastUpdateDate = parentFolder.LastUpdateDate,               
            };
        }

        private const string RootFolderName = "[Root]";
        private const string ParentFolderName = "[..]";
        private const string ParentFolderDesc = "Step up";
        private string _CurrentFolderName;
        private ObservableCollection<FileSystemItemDescriptor> _Items = new ObservableCollection<FileSystemItemDescriptor>();
        private FileSystemItemDescriptor _SelectedItem;
        private IVirtualFileSystemRepository _Repository;
    }
}
