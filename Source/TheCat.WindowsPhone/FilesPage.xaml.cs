using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TheCat.WindowsPhone.Concrete;
using TheCat.Infrastructure.VirtualFileSystem.Views;
using TheCat.Infrastructure;
using TheCat.Infrastructure.VirtualFileSystem;
using TheCat.Infrastructure.Events;
using TheCat.Infrastructure.VirtualFileSystem.Events;

namespace TheCat.WindowsPhone
{
    public partial class FilesPage : PhoneApplicationPage
    {
        public FilesPage()
        {
            InitializeComponent();

            INavigationManager navigationManager = Locator.Get<INavigationManager>();
            DataContext = FilesViewModel = Locator.Get<FilesViewModel>(navigationManager.PopTarget(), navigationManager.PopParams());
        }

        public FilesViewModel FilesViewModel { get; set; }

        private void TheFilesPage_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO
            if (FirstListBox.SelectedItem != null)
                FirstListBox.ScrollIntoView(FirstListBox.SelectedItem);
        }
    }
}