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
    public static class Lazy<T> where T : class, new()
    {
        public static T Value
        {
            get { return _Value ?? (_Value = new T()); }
        }

        public static bool HasValue
        {
            get { return _Value != null; }
        }

        private static T _Value = null;
    }
}
