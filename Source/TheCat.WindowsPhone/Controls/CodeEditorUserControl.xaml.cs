using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace TheCat.WindowsPhone.Controls
{
    public partial class CodeEditorUserControl : UserControl
    {
        public CodeEditorUserControl()
        {
            InitializeComponent();
            IsEditing = false;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CodeEditorUserControl),
            new PropertyMetadata("", new PropertyChangedCallback(TextChanged)));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(CodeEditorUserControl), 
            new PropertyMetadata(null));

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            CodeEditorUserControl control = (CodeEditorUserControl)o;
            string text = (string)e.NewValue;
            control.Items = text.Split(Separator, StringSplitOptions.None);
        }    

        public IEnumerable<string> Items
        {
            get { return GetValue(ItemsProperty) as IEnumerable<string>; }
            set { SetValue(ItemsProperty, value); }
        }

        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                _IsEditing = value;
                CodeTextBox.Visibility = IsEditing ? Visibility.Visible : Visibility.Collapsed;
                CodeListBox.Visibility = IsEditing ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public void Commit()
        {
            BindingExpression expression = CodeTextBox.GetBindingExpression(TextBox.TextProperty);
            if (expression != null) 
                expression.UpdateSource();
        }

        private void TextBlock_Tap(object sender, GestureEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            Point point = e.GetPosition(textBlock);
            int position = GetCaretPosition(point, textBlock.ActualWidth);
            CodeTextBox.Select(position, 0);
            CodeListBox.SelectedIndex = -1; // In order to hide the selection
        }

        private void CodeTextBox_LayoutUpdated(object sender, EventArgs e)
        {
            if (IsEditing)
                CodeTextBox.Focus();
        }

        private void CodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
        }

        private void CodeListBox_Tap(object sender, GestureEventArgs e)
        {
            if (CodeListBox.SelectedIndex != -1)
            {
                int position = Items.Take(CodeListBox.SelectedIndex + 1).Sum(s => s.Length + 2);
                if (position > 2)
                    position = position - 2; // Remove the last NewLine if it exists
                IsEditing = true;
                CodeTextBox.Select(position, 0);
                CodeListBox.SelectedIndex = -1; // In order to hide the selection
            }
        }

        public int GetCaretPosition(Point point, double textBlockActualWidth)
        {
            // Note: this approach works for monospace fonts and unwrapped rows
            string row = Items.ElementAt(CodeListBox.SelectedIndex);
            int position = (int)Math.Round(point.X / (textBlockActualWidth / row.Length));
            int finalPos = Items.Take(CodeListBox.SelectedIndex).Sum(s => s.Length + 2) + position;

            IsEditing = true;
            return finalPos;
        }

        private static readonly string[] Separator = new string[1] { Environment.NewLine };
        private bool _IsEditing;
    }
}
