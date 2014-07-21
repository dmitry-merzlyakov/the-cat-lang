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

namespace TheCat.Infrastructure.Validation.Conditions
{
    public abstract class BaseCompositeCondition<T> : ICondition<T>
    {
        public BaseCompositeCondition(params ICondition<T>[] conditions)
        {
            Conditions = conditions;
        }

        public IEnumerable<ICondition<T>> Conditions { get; private set; }

        public abstract bool Matches(T context);
    }
}
