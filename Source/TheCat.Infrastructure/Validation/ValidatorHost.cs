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

namespace TheCat.Infrastructure.Validation
{
    public sealed class ValidatorHost
    {
        public void AddValidator(Type hostClassName, IValidator validator)
        {
            if (hostClassName == null)
                throw new ArgumentNullException("hostClassName");

            if (validator == null)
                throw new ArgumentNullException("validator");

            CompositeValidator compositeValidator = GetCompositeValidator(hostClassName);
            if (compositeValidator == null)
                Validators[hostClassName] = compositeValidator = new CompositeValidator();

            compositeValidator.AddValidator(validator);
        }

        public CompositeValidator GetCompositeValidator(Type hostClassName)
        {
            CompositeValidator validator = null;
            Validators.TryGetValue(hostClassName, out validator);
            return validator;
        }

        public bool HasValidator(Type hostClassName)
        {
            return GetCompositeValidator(hostClassName) != null;
        }

        private readonly IDictionary<Type, CompositeValidator> Validators = new Dictionary<Type, CompositeValidator>();
    }
}
