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

namespace TheCat.WindowsPhone
{
    public partial class EditFileDescPage : PhoneApplicationPage
    {
        public EditFileDescPage()
        {
            InitializeComponent();
            INavigationManager navigationManager = Locator.Get<INavigationManager>();
            DataContext = FileDescriptorViewModel = Locator.Get<BaseFileDescriptorViewModel>(navigationManager.PopTarget(), navigationManager.PopParams());
        }

        public BaseFileDescriptorViewModel FileDescriptorViewModel { get; set; }
    }
}