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
using TheCat.Infrastructure.Sessions.Views;

namespace TheCat.WindowsPhone
{
    public partial class EditSessionPage : PhoneApplicationPage
    {
        public EditSessionPage()
        {
            InitializeComponent();
            INavigationManager navigationManager = Locator.Get<INavigationManager>();
            DataContext = SessionDefinitionsEditModel = Locator.Get<SessionDefinitionsEditModel>(navigationManager.PopTarget(), navigationManager.PopParams());
        }

        public SessionDefinitionsEditModel SessionDefinitionsEditModel { get; set; }
    }
}