using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace McMDK2.Core
{
    public class MetroDialogWrapper
    {
        public static async Task<LoginDialogData> ShowLoginDialog(string title, string message, LoginDialogSettings settings = null)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window != null)
            {
                return await window.ShowLoginAsync(title, message, settings);
            }
            return null;
        }

        public static async Task<string> ShowInputDialog(string title, string message, MetroDialogSettings settings = null)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window != null)
            {
                return await window.ShowInputAsync(title, message, settings);
            }
            return null;
        }

        public static async Task<MessageDialogResult> ShowMessageDialog(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window != null)
            {
                return await window.ShowMessageAsync(title, message, style, settings);
            }
            return MessageDialogResult.Negative;
        }

        public static async Task<ProgressDialogController> ShowProgressDialog(string title, string message, bool isCancelable = false, MetroDialogSettings settings = null)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window != null)
            {
                return await window.ShowProgressAsync(title, message, isCancelable, settings);
            }
            return null;
        }

        public static async Task ShowMetroDialog(BaseMetroDialog dialog)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window != null)
            {
                await window.ShowMetroDialogAsync(dialog);
            }
        }

        public static async Task HideMetroDialog(BaseMetroDialog dialog)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window != null)
            {
                await window.HideMetroDialogAsync(dialog);
            }
        }
    }
}
