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
    public class AndCondition<T> : BaseCompositeCondition<T>
    {
        public AndCondition(params ICondition<T>[] conditions) 
            : base (conditions)
        { }

        public override bool Matches(T context)
        {
            return Conditions.All(c => c.Matches(context));
        }
    }
}
