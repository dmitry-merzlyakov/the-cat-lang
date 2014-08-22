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
using TheCat.Infrastructure;
using TheCat.Infrastructure.Common;

namespace TheCat.WindowsPhone
{
    public partial class EditSettingsPage : PhoneApplicationPage
    {
        public EditSettingsPage()
        {
            InitializeComponent();
            INavigationManager navigationManager = Locator.Get<INavigationManager>();
            navigationManager.PopTarget(); navigationManager.PopParams();  // TODO - refactor it...
            DataContext = SettingsViewModel = Locator.Get<SettingsViewModel>();
        }

        public SettingsViewModel SettingsViewModel { get; set; }
    }
}