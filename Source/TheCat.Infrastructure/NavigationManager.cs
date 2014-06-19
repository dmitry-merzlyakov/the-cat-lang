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
        void Navigate(string target, params object[] parameters);
    }

    public static class NavigationManager
    {
        public static INavigationManager Current
        {
            get
            {
                if (_Current == null)
                    throw new InvalidOperationException("NavigationManager is not configured");

                return _Current;
            }
        }

        public static void Configure(INavigationManager manager)
        {
            _Current = manager;
        }

        private static INavigationManager _Current;
    }
}
