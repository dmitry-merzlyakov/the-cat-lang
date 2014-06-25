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
using TheCat.Infrastructure.VirtualFileSystem.Views;
using TheCat.WindowsPhone.Concrete;

namespace TheCat.WindowsPhone
{
    public partial class EditFileView : PhoneApplicationPage
    {
        public EditFileView()
        {
            InitializeComponent();

            string fileName = (string)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EditFileView.xaml"];
            GlobalObjects.EnsureInitialized();
            EditFileViewModel = new EditFileViewModel(GlobalObjects.VirtualFileSystemRepository, fileName);

            DataContext = EditFileViewModel;

            // TODO!!!
            //CodeEditor.Text = EditFileViewModel.Content;

            PagePivot.SelectedItem = PivotItemContent;
        }

        public EditFileViewModel EditFileViewModel { get; set; }
    }
}