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
using System.Windows.Data;

namespace TheCat.WindowsPhone.Controls
{
    public class TextBoxUpdateBindingBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.TextChanged += new TextChangedEventHandler(TextBox_TextChanged);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged -= new TextChangedEventHandler(TextBox_TextChanged);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression bindingExpression = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);

            if (bindingExpression != null)
                bindingExpression.UpdateSource();
        }
    }
}
