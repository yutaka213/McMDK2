using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Models;
using McMDK2.Views.TabPages;

namespace McMDK2.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            this.Tabs = new ObservableCollection<TabItem>();
        }

        public void Initialize()
        {
            TabItem startPage = new TabItem { Header = "Start" };
            startPage.Content = new StartPage();
            this.Tabs.Add(startPage);
        }


        #region NewWizardCommand
        private ViewModelCommand _NewWizardCommand;

        public ViewModelCommand NewWizardCommand
        {
            get
            {
                if (_NewWizardCommand == null)
                {
                    _NewWizardCommand = new ViewModelCommand(NewWizard);
                }
                return _NewWizardCommand;
            }
        }

        public void NewWizard()
        {
            Messenger.Raise(new TransitionMessage("ShowNewWizard"));
        }
        #endregion


        #region CloseCommand
        private ListenerCommand<object> _CloseCommand;

        public ListenerCommand<object> CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new ListenerCommand<object>(Close);
                }
                return _CloseCommand;
            }
        }

        public void Close(object parameter)
        {
            this.Tabs.Remove((TabItem)parameter);
        }
        #endregion


        #region Tabs変更通知プロパティ
        private ObservableCollection<TabItem> _Tabs;

        public ObservableCollection<TabItem> Tabs
        {
            get
            { return _Tabs; }
            set
            {
                if (_Tabs == value)
                    return;
                _Tabs = value;
                RaisePropertyChanged();
            }
        }
        #endregion


    }
}
