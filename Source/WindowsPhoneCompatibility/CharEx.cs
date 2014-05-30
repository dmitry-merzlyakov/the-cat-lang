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

namespace System
{
    public class CharEx
    {
        public static char Parse(string s)
        {
            char ch;
            if (!char.TryParse(s, out ch))
                throw new ArgumentException(s);
            return ch;
        }

    }
}
