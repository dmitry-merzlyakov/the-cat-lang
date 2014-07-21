using System;
using System.Linq;
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
    public sealed class OrCondition<T> : BaseCompositeCondition<T>
    {
        public OrCondition(params ICondition<T>[] conditions) 
            : base (conditions)
        { }

        public override bool Matches(T context)
        {
            return Conditions.Any(c => c.Matches(context));
        }
    }
}
