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

namespace TheCat.Infrastructure.Validation.ValueProviders
{
    public class FuncValueProvider<T,R> : IValueProvider<T,R>
    {
        public FuncValueProvider(Func<T,R> getter)
        {
            if (getter == null)
                throw new ArgumentNullException("getter");

            Getter = getter;
        }

        public Func<T,R> Getter { get; private set; }

        public R GetValue(T context)
        {
            return Getter(context);
        }
    }
}
