using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

using ICSharpCode.AvalonEdit;

namespace Fireworks.Behaviors
{
    public class TextEditorBindSupportBehavior : Behavior<TextEditor>
    {

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text",
            typeof(string), typeof(TextEditorBindSupportBehavior), new FrameworkPropertyMetadata(String.Empty, OnPropertyChangedCallback) { BindsTwoWayByDefault = true });

        private static void OnPropertyChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var behavior = target as TextEditorBindSupportBehavior;
            if (behavior == null)
            {
                return;
            }

            var texteditor = behavior.AssociatedObject as TextEditor;
            if (texteditor != null)
            {
                if (e.NewValue == null)
                {
                    return;
                }
                if (texteditor.Document != null)
                {
                    var offset = texteditor.CaretOffset;
                    texteditor.Document.Text = (string)e.NewValue;
                    texteditor.CaretOffset = offset;
                }
            }
        }

        public void TextChangedEvent(object sender, EventArgs e)
        {
            var textEditor = sender as TextEditor;
            if (textEditor != null)
                if (textEditor.Document != null)
                    Text = textEditor.Document.Text;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.TextChanged += TextChangedEvent;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.TextChanged -= TextChangedEvent;
        }
    }
}
