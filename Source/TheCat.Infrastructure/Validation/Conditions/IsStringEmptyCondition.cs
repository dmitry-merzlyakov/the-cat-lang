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
    public class IsStringEmptyCondition<T> : BaseValueCondition<T, string>
    {
        public IsStringEmptyCondition(IValueProvider<T, string> valueProvider)
            : base(valueProvider)
        { }

        protected override bool Matches(T context, string value)
        {
            return !String.IsNullOrWhiteSpace(value);
        }
    }
}
