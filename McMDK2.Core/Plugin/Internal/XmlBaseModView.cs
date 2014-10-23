using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using McMDK2.Plugin;
using McMDK2.UI;
using McMDK2.UI.Controls;

namespace McMDK2.Core.Plugin.Internal
{
    internal class XmlBaseModView : ModView
    {
        private List<UIControl> controls;
        private List<FrameworkElement> requiredControls;

        public XmlBaseModView(List<UIControl> controls)
        {
            this.controls = controls;
            this.requiredControls = new List<FrameworkElement>();
        }

        // Add controls to System.Windows.Controls.ContentControl.
        public void Build()
        {
            foreach (var control in this.controls)
            {
                var c = this.CreateControl(control.Component);
                this.Decorate(c, control);

                if (control is PanelControl)/* Have children controls. */
                    this.RecursiveBuild(c, (PanelControl)control);

                if (control is GroupBoxControl)/* Have child control. */
                    this.RecursiveBuild(c, (GroupBoxControl)control);

                this.AddChild(c);
            }
            //this.Content = 
        }

        private void RecursiveBuild(UIElement element, PanelControl control)
        {
            foreach (var innerControl in control.Children)
            {
                var c = this.CreateControl(innerControl.Component);
                this.Decorate(c, innerControl);

                if (innerControl is PanelControl)/* Have children controls. */
                    this.RecursiveBuild(c, (PanelControl)innerControl);

                if (innerControl is GroupBoxControl)/* Have child control. */
                    this.RecursiveBuild(c, (GroupBoxControl)innerControl);

                ((Panel)element).Children.Add(c);
            }
        }

        private void RecursiveBuild(UIElement element, GroupBoxControl control)
        {
            foreach (var innerControl in control.Children)
            {
                var c = this.CreateControl(innerControl.Component);
                this.Decorate(c, innerControl);

                if (innerControl is PanelControl)/* Have children controls. */
                    this.RecursiveBuild(c, (PanelControl)innerControl);

                if (innerControl is GroupBoxControl)/* Have child control. */
                    this.RecursiveBuild(c, (GroupBoxControl)innerControl);

                ((GroupBox)element).Content = c;
            }
        }

        private FrameworkElement CreateControl(GuiComponents component)
        {
            switch (component)
            {
                case GuiComponents.Canvas:
                    return new Canvas();

                case GuiComponents.CheckBox:
                    return new CheckBox();

                case GuiComponents.ComboBox:
                    return new ComboBox();

                case GuiComponents.DockPanel:
                    return new DockPanel();

                case GuiComponents.Grid:
                    return new Grid();

                case GuiComponents.GroupBox:
                    return new GroupBox();

                case GuiComponents.Image:
                    return new Image();

                case GuiComponents.Label:
                    return new Label();

                case GuiComponents.Null:
                    return new FrameworkElement();

                case GuiComponents.ScrollViewer:
                    return new ScrollViewer();

                case GuiComponents.Separator:
                    return new Separator();

                case GuiComponents.StackPanel:
                    return new DockPanel();

                case GuiComponents.TextBlock:
                    return new TextBlock();

                case GuiComponents.TextBox:
                    return new TextBox();

                case GuiComponents.UniformGrid:
                    return new System.Windows.Controls.Primitives.UniformGrid();

                case GuiComponents.WrapPanel:
                    return new WrapPanel();
            }
            return new FrameworkElement();
        }

        private void Decorate(FrameworkElement element, UIControl control)
        {
            this.DecorateUIControl(element, control);

            if (control is UIControl)
                this.DecorateUIControlEx((Control)element, (UIControlEx)control);

            if (control is PanelControl)
                this.DecoratePanelControl((Panel)element, (PanelControl)control);

            if (control is TextBlockControl)
                this.DecorateTextBlockControl((TextBlock)element, (TextBlockControl)control);

            if (control is SelectableControl)
                this.DecorateSelectableControl((ComboBox)element, (SelectableControl)control);

            if (control is GroupBoxControl)
                this.DecorateGroupBoxControl((GroupBox)element, (GroupBoxControl)control);

            if (control is EnterableControl)
                this.DecorateEnterableControl(element, (EnterableControl)control);

            if (control is McMDK2.UI.Controls.ContentControl)
                this.DecorateContentControl((System.Windows.Controls.ContentControl)element, (McMDK2.UI.Controls.ContentControl)control);
        }

        private void DecorateUIControl(FrameworkElement element, UIControl control)
        {
            element.IsEnabled = control.IsEnabled;

            // readonly
            //if (control.IsVisible != null)
            //    element.IsVisible = control.IsVisible;

            if (control.Opacity != -1.0)
                element.Opacity = control.Opacity;

            if (control.Visibility != null)
                element.Visibility = (Visibility)control.Visibility;

            if (control.Height != -1.0)
                element.Height = control.Height;

            if (control.HorizontalAlignment != null)
                element.HorizontalAlignment = (HorizontalAlignment)control.HorizontalAlignment;

            if (control.Margin != null)
                element.Margin = (Thickness)control.Margin;

            if (control.Name != null)
                element.Name = control.Name;

            if (control.ToolTip != null)
                element.ToolTip = control.ToolTip;

            if (control.VerticalAlignment != null)
                element.VerticalAlignment = (VerticalAlignment)control.VerticalAlignment;

            if (control.Width != -1.0)
                element.Width = control.Width;
        }

        private void DecorateUIControlEx(Control element, UIControlEx control)
        {
            if (control.Background != null)
                element.Background = (Brush)control.Background;

            if (control.BorderBrush != null)
                element.BorderBrush = (Brush)control.BorderBrush;

            if (control.BorderThickess != null)
                element.BorderThickness = (Thickness)control.BorderThickess;

            if (control.FontFamily != null)
                element.FontFamily = (FontFamily)control.FontFamily;

            if (control.FontSize != -1.0)
                element.FontSize = control.FontSize;

            if (control.FontStretch != null)
                element.FontStretch = (FontStretch)control.FontStretch;

            if (control.FontWeight != null)
                element.FontWeight = (FontWeight)control.FontWeight;

            if (control.Foreground != null)
                element.Foreground = (Brush)control.Foreground;

            if (control.Padding != null)
                element.Padding = (Thickness)control.Padding;
        }

        private void DecoratePanelControl(Panel panel, PanelControl control)
        {
            if (control.Backgound != null)
                panel.Background = (Brush)control.Backgound;
        }

        private void DecorateTextBlockControl(TextBlock textBlock, TextBlockControl control)
        {
            if (control.Background != null)
                textBlock.Background = (Brush)control.Background;

            if (control.FontFamiy != null)
                textBlock.FontFamily = (FontFamily)control.FontFamiy;

            if (control.FontSize != -1.0)
                textBlock.FontSize = control.FontSize;

            if (control.FontStretch != null)
                textBlock.FontStretch = (FontStretch)control.FontStretch;

            if (control.FontStyle != null)
                textBlock.FontStyle = (FontStyle)control.FontStyle;

            if (control.FontWeight != null)
                textBlock.FontWeight = (FontWeight)control.FontWeight;

            if (control.Foreground != null)
                textBlock.Foreground = (Brush)control.Foreground;

            if (control.Padding != null)
                textBlock.Padding = (Thickness)control.Padding;

            if (control.LineHeight != -1.0)
                textBlock.LineHeight = control.LineHeight;

            if (control.Text != null)
                textBlock.Text = control.Text;

            if (control.TextAlignment != null)
                textBlock.TextAlignment = (TextAlignment)control.TextAlignment;

            if (control.TextDecorations != null)
                textBlock.TextDecorations.Add((TextDecoration)control.TextDecorations);

            if (control.TextTrimming != null)
                textBlock.TextTrimming = (TextTrimming)control.TextTrimming;

            if (control.TextWrapping != null)
                textBlock.TextWrapping = (TextWrapping)control.TextWrapping;
        }

        private void DecorateSelectableControl(ComboBox/* THIS CODE SHOULD REMOVE. */ comboBox, SelectableControl control)
        {
            //if(control.ItemsSource != null)
            //    comboBox.ItemsSource = ;
        }

        private void DecorateGroupBoxControl(GroupBox groupBox, GroupBoxControl control)
        {
            if (control.Header != null)
                groupBox.Header = control.Header;
        }

        private void DecorateEnterableControl(FrameworkElement element, EnterableControl control)
        {
            if (control.IsRequired)
                this.requiredControls.Add(element);

            if (control.Default != null)
            {
                switch (control.Component)
                {
                    case GuiComponents.CheckBox:
                        var checkBox = (CheckBox)element;
                        checkBox.IsChecked = (bool)control.Default;
                        break;

                    case GuiComponents.ComboBox:
                        var comboBox = (ComboBox)element;
                        comboBox.SelectedItem = control.Default;
                        break;

                    case GuiComponents.TextBox:
                        var textBox = (TextBox)element;
                        textBox.Text = (string)control.Default;
                        break;
                }
            }
        }

        private void DecorateContentControl(System.Windows.Controls.ContentControl element, McMDK2.UI.Controls.ContentControl control)
        {
            if (control.Content != null)
                element.Content = control.Content;
        }
    }
}
