using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Models;
using McMDK2.Plugin;
using McMDK2.Views.SettingPages;

namespace McMDK2.ViewModels
{
    public class SettingWindowViewModel : ViewModel
    {
        private Dictionary<string, UserControl> views = new Dictionary<string, UserControl>();

        public SettingWindowViewModel()
        {
            this.views.Add("General", new GeneralPage());
            this.views.Add("Environment", new EnvironmentPage());
            this.views.Add("Browser", new BrowserPage());
            this.views.Add("Java", new JavaPage());

            this.CurrentSettingView = this.views["General"];

        }

        public void Initialize()
        {
            foreach (var item in this.views)
            {
                var configuration = item.Value.DataContext as IConfiguration;
                if (configuration != null)
                {
                    ((IConfiguration)item.Value.DataContext).Load();
                }
            }
        }


        #region OkCommand
        private ViewModelCommand _OkCommand;

        public ViewModelCommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                {
                    _OkCommand = new ViewModelCommand(Ok);
                }
                return _OkCommand;
            }
        }

        public void Ok()
        {
            foreach (var item in this.views)
            {
                var configuration = item.Value.DataContext as IConfiguration;
                if (configuration != null)
                {
                    ((IConfiguration)item.Value.DataContext).Apply();
                }
            }
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }
        #endregion


        #region CancelCommand
        private ViewModelCommand _CancelCommand;

        public ViewModelCommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new ViewModelCommand(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel()
        {
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }
        #endregion


        #region SelectedItem変更通知プロパティ
        private object _SelectedItem;

        public object SelectedItem
        {
            get
            { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                this.CurrentSettingView = this.views[(string)((TreeViewItem)_SelectedItem).Tag];
                RaisePropertyChanged();
            }
        }
        #endregion


        #region PluginViews変更通知プロパティ
        private ObservableCollection<IPlugin> _PluginViews;

        public ObservableCollection<IPlugin> PluginViews
        {
            get
            { return _PluginViews; }
            set
            {
                if (_PluginViews == value)
                    return;
                _PluginViews = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region CurrentSettingView変更通知プロパティ
        private UserControl _CurrentSettingView;

        public UserControl CurrentSettingView
        {
            get
            { return _CurrentSettingView; }
            set
            {
                if (_CurrentSettingView == value)
                    return;
                _CurrentSettingView = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
