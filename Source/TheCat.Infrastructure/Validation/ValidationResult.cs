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
using System.Linq;

namespace TheCat.Infrastructure.Validation
{
    public sealed class ValidationResult<T> : IValidationResult
    {
        public ValidationResult(IEnumerable<ValidationRule<T>> failedRules)
        {
            FailedRules = failedRules ?? Enumerable.Empty<ValidationRule<T>>();
        }

        public IEnumerable<ValidationRule<T>> FailedRules { get; private set; }

        public bool IsValid
        {
            get { return !FailedRules.Any(); }
        }

        IEnumerable<string> IValidationResult.Messages
        {
            get { return FailedRules.Select(fr => fr.Message); }
        }
    }
}
