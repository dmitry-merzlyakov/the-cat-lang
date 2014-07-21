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

namespace TheCat.Infrastructure.Validation
{
    public sealed class ValidationRule<T>
    {
        public ValidationRule(ICondition<T> condition, string message)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (String.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message");

            Condition = condition;
            Message = message;
        }

        public ICondition<T> Condition { get; private set; }
        public string Message { get; private set; }

        public bool Validate(T context)
        {
            return Condition.Matches(context);
        }
    }
}
