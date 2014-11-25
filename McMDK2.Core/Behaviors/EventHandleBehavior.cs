using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using McMDK2.Core.Behaviors.Internal;

#pragma warning disable 1591

namespace McMDK2.Core.Behaviors
{
    /// <summary>
    /// 文字列によるEventのHookを行います。
    /// </summary>
    public class EventHandleBehavior
    {
        public static DependencyProperty EventNameProperty = DependencyProperty.RegisterAttached("EventName", typeof(string), typeof(EventHandleBehavior), new UIPropertyMetadata(PropertyChanged));

        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventHandleBehavior), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static void SetEventName(DependencyObject target, string value)
        {
            target.SetValue(EventNameProperty, value);
        }

        public static string GetEventName(DependencyObject target)
        {
            return (string)target.GetValue(EventNameProperty);
        }

        private static void PropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as UIElement;
            if (element != null)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    var type = typeof(EventHandler);
                    var info = typeof(EventHandleBehavior).GetMethod("OnEventHanding");
                    var _d = Delegate.CreateDelegate(type, info);
                    try
                    {
                        // EventHandlerで通せない場合は、例外キャッチ先で適切な方でリフレクションを行います。
                        element.GetType().GetEvent((string)e.NewValue).AddEventHandler(element, _d);
                    }
                    catch (Exception e_)
                    {
                        string typeName = GetTargetHandler(e_.Message);
                        _d = Delegate.CreateDelegate(AsmResolver.GetTypeFromString(typeName), info);
                        element.GetType().GetEvent((string)e.NewValue).AddEventHandler(element, _d);
                    }
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    var type = typeof(EventHandler);
                    var info = typeof(EventHandleBehavior).GetMethod("OnEventHanding");
                    var _d = Delegate.CreateDelegate(type, info);
                    try
                    {
                        element.GetType().GetEvent((string)e.OldValue).RemoveEventHandler(element, _d);
                    }
                    catch (Exception e_)
                    {
                        string typeName = GetTargetHandler(e_.Message);
                        _d = Delegate.CreateDelegate(AsmResolver.GetTypeFromString(typeName), info);
                        element.GetType().GetEvent((string)e.NewValue).RemoveEventHandler(element, _d);
                    }
                }
            }
        }

        public static void OnEventHanding(object sender, EventArgs e)
        {
            var element = sender as UIElement;
            var command = (ICommand)element.GetValue(CommandProperty);
            command.Execute(new object[] { sender, e });
        }

        private static string GetTargetHandler(string text)
        {
            // 英語環境わかりません。
            string r = RegExp.Do(text, "型 '.*' のオブジェクトを型 '(?<Target>.*)' に変換できません。", System.Text.RegularExpressions.RegexOptions.IgnoreCase).ToArray()[0].Value;
            return r;
        }
    }
}
