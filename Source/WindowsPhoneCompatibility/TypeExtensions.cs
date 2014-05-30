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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetNestedTypes(this Type t)
        {
            return t.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }
    }
}
