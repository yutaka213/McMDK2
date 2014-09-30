using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace McMDK2.Core.Behaviors
{
    /// <summary>
    /// http://nishantrana.me/2012/03/13/creating-dynamic-contextmenu-for-treeview-in-wpf/
    /// Modified.
    /// </summary>
    public class DynamicContextMenuBehavior
    {

        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DynamicContextMenuBehavior), new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(DynamicContextMenuBehavior), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static void SetCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }

        public static object GetCommandParameter(DependencyObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as UIElement;
            if (element != null)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    element.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    element.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
                }
            }
        }

        private static void OnPreviewMouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            var element = sender as UIElement;
            var command = (ICommand)element.GetValue(CommandProperty);
            object commandParameter = element.GetValue(CommandParameterProperty);
            object parameter = (object)(new object[] { sender, e, commandParameter });
            command.Execute(parameter);
        }
    }
}
