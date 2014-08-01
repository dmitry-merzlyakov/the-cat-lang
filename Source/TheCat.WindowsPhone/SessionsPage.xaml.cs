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
using TheCat.Infrastructure.Sessions.Views;
using TheCat.Infrastructure;

namespace TheCat.WindowsPhone
{
    public partial class SessionsPage : PhoneApplicationPage
    {
        public SessionsPage()
        {
            InitializeComponent();

            INavigationManager navigationManager = Locator.Get<INavigationManager>();
            DataContext = SessionDefinitionsViewModel = Locator.Get<SessionDefinitionsViewModel>(navigationManager.PopTarget(), navigationManager.PopParams());
        }

        public SessionDefinitionsViewModel SessionDefinitionsViewModel { get; set; }
    }
}