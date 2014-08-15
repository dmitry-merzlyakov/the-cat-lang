﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TheCat.Infrastructure
{
    public class Command : ICommand
    {
        public Command(Action simpleAction, bool canExecute = true)
        {
            SimpleAction = simpleAction;
            CanExecute = canExecute;
        }

        public Command(Action<object> paramAction, bool canExecute = true)
        {
            ParamAction = paramAction;
            CanExecute = canExecute;
        }

        public bool CanExecute
        {
            get { return _CanExecute; }
            set
            {
                if (_CanExecute != value)
                {
                    _CanExecute = value;
                    if (CanExecuteChanged != null)
                        Deployment.Current.Dispatcher.BeginInvoke(() => CanExecuteChanged(this, EventArgs.Empty));
                }
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute;
        }

        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            if (CanExecute)
            {
                if (SimpleAction != null)
                    SimpleAction();
                else
                    if (ParamAction != null)
                        ParamAction(parameter);
            }
        }

        private readonly Action SimpleAction;
        private readonly Action<object> ParamAction;
        private bool _CanExecute;
    }

}
