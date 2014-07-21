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
    public sealed class CompositeValidator : IValidator
    {
        public void AddValidator(IValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            Validators.Add(validator);
        }

        public IValidationResult Validate(object context)
        {
            return new CompositeValidationResult(Validators.Select(v => v.Validate(context)).Where(vr => !vr.IsValid));
        }

        private readonly IList<IValidator> Validators = new List<IValidator>();
    }
}
