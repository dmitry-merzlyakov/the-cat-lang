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

namespace TheCat.Infrastructure.Validation.Conditions
{
    public abstract class BaseValueCondition<T,R> : ICondition<T>
    {
        public BaseValueCondition(IValueProvider<T,R> valueProvider)
        {
            if (valueProvider == null)
                throw new ArgumentNullException("valueProvider");

            ValueProvider = valueProvider;
        }

        public IValueProvider<T,R> ValueProvider { get; private set; }

        public bool Matches(T context)
        {
            return Matches(context, ValueProvider.GetValue(context));
        }

        protected abstract bool Matches(T context, R value);
    }
}
