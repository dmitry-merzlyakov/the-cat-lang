using System;
using System.Linq;
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
using TheCat.Infrastructure.Events;
using TheCat.Infrastructure.VirtualFileSystem.Events;

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public class FilesViewModel : INotifyPropertyChanged
    {
        public FilesViewModel(IVirtualFileSystemRepository repository, string fullFolderName)
        {
            Repository = repository;
            CurrentFolder = Repository.GetFolder(fullFolderName);
            RefreshItems();

            CreateFolderCommand = new Command(() => Locator.Get<INavigationManager>().Navigate(StringKeys.CreateFolder, CompositeParams.Create(StringKeys.CurrentFolderName, CurrentFolderFullName)));
            CreateFileCommand = new Command(() => Locator.Get<INavigationManager>().Navigate(StringKeys.CreateFile, CompositeParams.Create(StringKeys.CurrentFolderName, CurrentFolderFullName)));
            OpenCommand = new Command((p) => OpenFileSystemItem((FileSystemItemDescriptor)p));
            RenameCommand = new Command((p) => RenameFileSystemItem((FileSystemItemDescriptor)p));
            CopyCommand = new Command((p) => CopyFileSystemItem((FileSystemItemDescriptor)p));
            CutCommand = new Command((p) => CutFileSystemItem((FileSystemItemDescriptor)p));
            PasteCommand = new ClipboardCommand(() => PasteFileSystemItem());
            ClearClipboardCommand = new ClipboardCommand(() => FileSystemItemClipboard.Current.Clear());
            DeleteCommand = new Command((p) => DeleteFileSystemItem((FileSystemItemDescriptor)p));

            Locator.Get<EventManager>().RegisterSubscription<ItemChangedEvent>(ItemChangedHandler);
            Locator.Get<EventManager>().RegisterSubscription<ClipboardChangedEvent>(ClipboardChangedHandler);
        }

        public void ItemChangedHandler(ItemChangedEvent evnt)
        {
            if (evnt.ModificationType == ItemModificationType.Deleted && CurrentFolder.FullName == evnt.Subject.FullName)
            {
                Locator.Get<EventManager>().UnregisterSubscription<ItemChangedEvent>(ItemChangedHandler);
                return;
            }

            if (evnt.Subject.ParentFolderName == CurrentFolder.FullName)
            {
                RefreshItems();
                SelectedItem = Items.FirstOrDefault(d => d.FullName == evnt.Subject.FullName);
            }
        }

        private void ClipboardChangedHandler(ClipboardChangedEvent evnt)
        {
            // TODO - try to find more efficient solution than simply refresh items
            if (Items.Any(d => d == evnt.ActualDescriptor || d == evnt.PreviousDescriptor))
                RefreshItems();
        }

        private void DeleteFileSystemItem(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");

            string caption = fileSystemItemDescriptor.IsFolder ? "Delete folder" : "Delete file";
            string text = String.Format("Do you really want to delete the {0} '{1}'?", fileSystemItemDescriptor.IsFolder ? "folder" : "file", fileSystemItemDescriptor.Name);

            if (MessageBox.Show(text, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                Repository.Delete(fileSystemItemDescriptor).ShowFileSystemError();
        }

        private void PasteFileSystemItem()
        {
            if (FileSystemItemClipboard.Current.Item != null)
            {
                FileSystemItemDescriptor descriptor = FileSystemItemClipboard.Current.Item;
                string name = descriptor.Name;

                switch (FileSystemItemClipboard.Current.ClipboardAction)
                {
                    case ClipboardActionEnum.Copy:
                        string copyMessage = String.Format("Do you want to put the copy of '{0}' to the folder '{1}?", FileSystemItemClipboard.Current.Item.Name, CurrentFolderName);
                        if (MessageBox.Show(copyMessage, "Copy", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            bool exists = Repository.GetFolderContent(CurrentFolderFullName).Any(d => d.Name == descriptor.Name);
                            if (exists)
                                name = Repository.GenerateUniqueName(CurrentFolderName, name);
                            Repository.Copy(descriptor, CurrentFolder, name).ShowFileSystemError();
                            FileSystemItemClipboard.Current.Clear();
                        }
                        break;

                    case ClipboardActionEnum.Cut:
                        if (descriptor.ParentFolderName == CurrentFolder.FullName)
                        {
                            MessageBox.Show("You cannot move an item to the same folder. Select another folder.");
                            return;
                        }

                        string moveMessage = String.Format("Do you want to move '{0}' to the folder '{1}?", FileSystemItemClipboard.Current.Item.Name, CurrentFolderName);
                        if (MessageBox.Show(moveMessage, "Move", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            bool exists = Repository.GetFolderContent(CurrentFolderFullName).Any(d => d.Name == descriptor.Name);
                            if (exists)
                                name = Repository.GenerateUniqueName(CurrentFolderName, name);
                            Repository.Move(descriptor, CurrentFolder, name).ShowFileSystemError();
                            FileSystemItemClipboard.Current.Clear();
                        }
                        break;

                    default:
                        throw new NotImplementedException(FileSystemItemClipboard.Current.ClipboardAction.ToString());
                }
            }
        }

        private void CutFileSystemItem(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");

            FileSystemItemClipboard.Current.SetItem(fileSystemItemDescriptor, ClipboardActionEnum.Cut);
        }

        private void CopyFileSystemItem(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor == null)
                throw new ArgumentNullException("fileSystemItemDescriptor");

            FileSystemItemClipboard.Current.SetItem(fileSystemItemDescriptor, ClipboardActionEnum.Copy);
        }

        private void OpenFileSystemItem(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor != null)
            {
                if (fileSystemItemDescriptor.IsFolder)
                    Locator.Get<INavigationManager>().Navigate(StringKeys.ViewFiles, CompositeParams.Create(StringKeys.CurrentFolderName, fileSystemItemDescriptor.FullName));
                else
                    Locator.Get<INavigationManager>().Navigate(StringKeys.EditFile, CompositeParams.Create(StringKeys.FileName, fileSystemItemDescriptor.FullName));
            }
        }

        private void RenameFileSystemItem(FileSystemItemDescriptor fileSystemItemDescriptor)
        {
            if (fileSystemItemDescriptor.IsFolder)
                Locator.Get<INavigationManager>().Navigate(StringKeys.ModifyFolder, CompositeParams.Create(StringKeys.FolderName, fileSystemItemDescriptor.FullName));
            else
                Locator.Get<INavigationManager>().Navigate(StringKeys.ModifyFile, CompositeParams.Create(StringKeys.FileName, fileSystemItemDescriptor.FullName));
        }

        public string CurrentFolderFullName
        {
            get { return CurrentFolder.FullName; }
        }

        public string CurrentFolderName
        {
            get { return CurrentFolder.Name; }
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
            }
        }

        public ICommand CreateFolderCommand { get; private set; }
        public ICommand CreateFileCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand RenameCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand ClearClipboardCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

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

        private void RefreshItems()
        {
            FileSystemItemDescriptor previouslySelected = SelectedItem;

            Items.Clear();
            foreach (FileSystemItemDescriptor item in Repository.GetFolderContent(CurrentFolder.FullName))
                Items.Add(item);

            if (Items.Contains(previouslySelected))
                SelectedItem = previouslySelected;
        }

        private ObservableCollection<FileSystemItemDescriptor> _Items = new ObservableCollection<FileSystemItemDescriptor>();
        private FileSystemItemDescriptor _SelectedItem;
        private IVirtualFileSystemRepository _Repository;
    }
}
