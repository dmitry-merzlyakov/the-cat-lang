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

namespace TheCat.Infrastructure
{
    public interface INavigationManager
    {
        void GoBack();
        void Navigate(string target, CompositeParams parms = null);

        string PopTarget();
        CompositeParams PopParams();
    }
}
