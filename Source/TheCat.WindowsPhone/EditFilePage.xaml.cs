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
    public partial class EditFilePage : PhoneApplicationPage
    {
        public EditFilePage()
        {
            InitializeComponent();

            string fileName = (string)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EditFilePage.xaml"];
            GlobalObjects.EnsureInitialized();
            EditFileViewModel = new EditFileViewModel(GlobalObjects.VirtualFileSystemRepository, fileName);
            DataContext = EditFileViewModel;
        }

        public EditFileViewModel EditFileViewModel { get; set; }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CodeEditor.Commit();
            if (EditFileViewModel.IsContentChanged)
            {
                // TODO - do we need to handle IO errors?
                EditFileViewModel.ValidateAndSave();
            }
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CodeEditor.Commit();
            if (CanExit())
                NavigationService.GoBack();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            e.Cancel = !CanExit();
        }

        private bool CanExit()
        {
            if (EditFileViewModel.IsContentChanged)
            {
                var result = MessageBox.Show("You are about to discard your changes. Continue?", "Warning", MessageBoxButton.OKCancel);
                return result != MessageBoxResult.Cancel;
            }
            return true;
        }
    }
}