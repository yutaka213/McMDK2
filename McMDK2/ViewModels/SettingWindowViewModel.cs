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

namespace McMDK2.ViewModels
{
    public class SettingWindowViewModel : ViewModel
    {

        public void Initialize()
        {
        }


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
