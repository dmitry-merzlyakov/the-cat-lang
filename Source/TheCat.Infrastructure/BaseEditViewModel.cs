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
using TheCat.Infrastructure.Validation;

namespace TheCat.Infrastructure
{
    public abstract class BaseEditViewModel : BaseViewModel
    {
        public BaseEditViewModel()
        {
            CommitCommand = new Command(CommitCommandHandler);
            CancelCommand = new Command(CancelCommandHandler);

            if (!Lazy<ValidatorHost>.Value.HasValidator(this.GetType()))
                InitializeValidator();
        }

        public ICommand CommitCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        protected virtual void InitializeValidator()
        { }

        protected void AddValidator(IValidator validator)
        {
            Lazy<ValidatorHost>.Value.AddValidator(this.GetType(), validator);
        }

        protected virtual bool Validate()
        {
            IValidator validator = Lazy<ValidatorHost>.Value.GetCompositeValidator(this.GetType());
            ValidationResult = validator != null ? validator.Validate(this) : null;
            return ValidationResult != null ? ValidationResult.IsValid : true;
        }

        protected virtual bool CommitChanges(ref string messages)
        {
            return true;
        }

        private IValidationResult ValidationResult { get; set; }

        private void CommitCommandHandler()
        {
            string message = null;

            if (Validate())
            {
                if (CommitChanges(ref message))
                {
                    Locator.Get<INavigationManager>().GoBack();
                    return;
                }
            }
            else
            {
                message = ValidationResult.GetFormattedMessage();
            }

            MessageBox.Show(message);
        }

        private void CancelCommandHandler()
        {
            Locator.Get<INavigationManager>().GoBack();
        }

    }
}
