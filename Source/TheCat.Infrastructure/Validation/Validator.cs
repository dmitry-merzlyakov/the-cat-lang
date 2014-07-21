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
    public sealed class Validator<T> : IValidator
    {
        public void AddValidationRule(ValidationRule<T> validationRule)
        {
            if (validationRule == null)
                throw new ArgumentNullException("validationRule");

            Rules.Add(validationRule);
        }

        public ValidationResult<T> Validate(T context)
        {
            return new ValidationResult<T>(Rules.Where(rule => !rule.Validate(context)));
        }

        IValidationResult IValidator.Validate(object context)
        {
            return this.Validate((T)context);
        }

        private readonly IList<ValidationRule<T>> Rules = new List<ValidationRule<T>>();
    }
}
