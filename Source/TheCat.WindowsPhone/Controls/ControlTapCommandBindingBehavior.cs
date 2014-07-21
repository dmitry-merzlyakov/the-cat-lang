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
using System.Windows.Interactivity;

namespace TheCat.WindowsPhone.Controls
{
    public class ControlTapCommandBindingBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ControlTapCommandBindingBehavior),
            new PropertyMetadata(null));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ControlTapCommandBindingBehavior),
            new PropertyMetadata(null));

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty) as object; }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Tap += new EventHandler<GestureEventArgs>(AssociatedObject_Tap);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Tap -= new EventHandler<GestureEventArgs>(AssociatedObject_Tap);
        }

        void AssociatedObject_Tap(object sender, GestureEventArgs e)
        {
            if (Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);
        }
    }
}
