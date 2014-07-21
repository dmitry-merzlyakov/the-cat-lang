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
using TheCat.Infrastructure.Validation.Conditions;
using TheCat.Infrastructure.Validation.ValueProviders;
using System.Text;

namespace TheCat.Infrastructure.Validation
{
    public static class Extensions
    {
        public static void AddIsNotEmptyRule<T>(this Validator<T> validator, Func<T,string> getter, string message = null)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (getter == null)
                throw new ArgumentNullException("getter");

            IValueProvider<T, string> valueProvider = new FuncValueProvider<T,string>(getter);
            IsStringEmptyCondition<T> condition = new IsStringEmptyCondition<T>(valueProvider);
            ValidationRule<T> validationRule = new ValidationRule<T>(condition, message ?? AddIsNotEmptyRuleDefaultMessage);

            validator.AddValidationRule(validationRule);
        }

        public static string GetFormattedMessage(this IValidationResult validationResult)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in validationResult.Messages)
                sb.AppendLine(s);
            return sb.ToString();
        }

        private const string AddIsNotEmptyRuleDefaultMessage = "Field is required";
    }
}
