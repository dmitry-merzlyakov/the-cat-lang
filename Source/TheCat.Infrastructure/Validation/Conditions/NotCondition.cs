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
    public sealed class NotCondition<T> : ICondition<T>
    {
        public NotCondition(ICondition<T> condition)
        { 
            if (condition == null)
                throw new ArgumentNullException("condition");

            Condition = condition;
        }

        public ICondition<T> Condition { get; private set; }

        public bool Matches(T context)
        {
            return !Condition.Matches(context);
        }
    }
}
