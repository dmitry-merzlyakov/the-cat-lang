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
using System.Collections.Generic;

namespace TheCat.Infrastructure.Validation
{
    public sealed class CompositeValidationResult : IValidationResult
    {
        public CompositeValidationResult(IEnumerable<IValidationResult> validationResults)
        {
            ValidationResults = validationResults ?? Enumerable.Empty<IValidationResult>();
        }

        public bool IsValid
        {
            get { return ValidationResults.All(vr => vr.IsValid); }
        }

        public IEnumerable<string> Messages
        {
            get { return ValidationResults.SelectMany(vr => vr.Messages) ; }
        }

        private readonly IEnumerable<IValidationResult> ValidationResults;
    }
}
