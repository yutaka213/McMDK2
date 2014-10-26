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
using McMDK2.Core.Plugin;
using McMDK2.Models;
using McMDK2.Plugin;
using McMDK2.ViewModels.TabPages;
using McMDK2.Views.TabPages;

namespace McMDK2.ViewModels
{
    public class NewItemWindowViewModel : ViewModel
    {
        public readonly MainWindowViewModel MainWindowViewModel;

        public NewItemWindowViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.Items = new ObservableCollection<IMod>(ModManager.Mods);
        }

        public void Initialize()
        {
        }


        #region OKCommand
        private ViewModelCommand _OKCommand;

        public ViewModelCommand OKCommand
        {
            get
            {
                if (_OKCommand == null)
                {
                    _OKCommand = new ViewModelCommand(OK, CanOK);
                }
                return _OKCommand;
            }
        }

        public void OK()
        {
            IMod mod = ModManager.GetModFromId(this.SelectedItem.Id);

            var moddingPage = new TabItem
            {
                Header = this.ItemName,
                Content = new ModdingPage { DataContext = new ModdingPageViewModel(mod.View) },
                Tag = Guid.NewGuid().ToString()
            };
            //((ModdingPage)moddingPage.Content).Draw();
            this.MainWindowViewModel.Tabs.Add(moddingPage);
            this.MainWindowViewModel.SelectedTabIndex = this.MainWindowViewModel.Tabs.IndexOf(moddingPage);

            this.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }

        public bool CanOK()
        {
            if (String.IsNullOrWhiteSpace(this.ItemName) || this.SelectedItem == null)
            {
                return false;
            }
            return true;
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
            this.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }
        #endregion


        #region Items変更通知プロパティ
        private ObservableCollection<IMod> _Items;

        public ObservableCollection<IMod> Items
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
        private IMod _SelectedItem;

        public IMod SelectedItem
        {
            get
            { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                this.SelectedItemName = _SelectedItem.Name;
                this.SelectedItemDescription = _SelectedItem.Description;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItemName変更通知プロパティ
        private string _SelectedItemName;

        public string SelectedItemName
        {
            get
            { return _SelectedItemName; }
            set
            {
                if (_SelectedItemName == value)
                    return;
                _SelectedItemName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItemDescription変更通知プロパティ
        private string _SelectedItemDescription;

        public string SelectedItemDescription
        {
            get
            { return _SelectedItemDescription; }
            set
            {
                if (_SelectedItemDescription == value)
                    return;
                _SelectedItemDescription = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ItemName変更通知プロパティ
        private string _ItemName;

        public string ItemName
        {
            get
            { return _ItemName; }
            set
            {
                if (_ItemName == value)
                    return;
                _ItemName = value;
                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


    }
}
