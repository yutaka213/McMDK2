using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core.Plugin;
using McMDK2.Models;
using McMDK2.Plugin;

namespace McMDK2.ViewModels.Dialogs
{
    public class PluginInfoDialogViewModel : ViewModel
    {
        public PluginInfoDialogViewModel()
        {
            this.Items = new ObservableCollection<IPlugin>();
        }

        public void Initialize()
        {
            this.Items = new ObservableCollection<IPlugin>(PluginManager.Plugins);
        }


        #region Items変更通知プロパティ
        private ObservableCollection<IPlugin> _Items;

        public ObservableCollection<IPlugin> Items
        {
            get
            { return _Items; }
            set
            {
                if (_Items == value)
                    return;
                _Items = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItem変更通知プロパティ
        private IPlugin _SelectedItem;

        public IPlugin SelectedItem
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


        #region CloseCommand
        private ViewModelCommand _CloseCommand;

        public ViewModelCommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new ViewModelCommand(Close);
                }
                return _CloseCommand;
            }
        }

        public void Close()
        {
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }
        #endregion

    }
}
