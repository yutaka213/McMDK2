using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace McMDK2.Core.Behaviors
{
    public class SelectedItemBindBehavior : Behavior<TreeView>
    {
        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(SelectedItemBindBehavior), new UIPropertyMetadata(null, PropertyChanged));

        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //
        }

        void AssociatedObject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = e.NewValue;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectedItemChanged += AssociatedObject_SelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectedItemChanged -= AssociatedObject_SelectedItemChanged;
        }
    }
}
