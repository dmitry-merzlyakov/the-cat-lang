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
using System.ComponentModel;
using System.Collections.Generic;
using TheCat.Infrastructure.Validation;

namespace TheCat.Infrastructure.VirtualFileSystem.Views
{
    public abstract class BaseFileDescriptorViewModel : BaseEditViewModel
    {
        public BaseFileDescriptorViewModel(IVirtualFileSystemRepository repository)
        {
            Repository = repository;
        }

        public string Title
        {
            get { return GetTitle(); }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        protected IVirtualFileSystemRepository Repository
        {
            get { return _Repository; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Repository = value;
            }
        }

        protected abstract string GetTitle();

        protected override void InitializeValidator()
        {
            base.InitializeValidator();

            Validator<BaseFileDescriptorViewModel> validator = new Validator<BaseFileDescriptorViewModel>();
            validator.AddIsNotEmptyRule(context => context.Name, "Name is required");
            AddValidator(validator);
        }

        private IVirtualFileSystemRepository _Repository;
        private string _Name;
        private string _Description;
    }
}
