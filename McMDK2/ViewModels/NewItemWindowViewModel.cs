using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Plugin;
using McMDK2.Models;
using McMDK2.Plugin;
using McMDK2.ViewModels.TabPages;
using McMDK2.Views.TabPages;
using Microsoft.WindowsAPICodePack.Dialogs;

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
            string id = Guid.NewGuid().ToString();

            var moddingPage = new TabItem
            {
                Header = this.ItemName + ".mod",
                Content = new ModdingPage { DataContext = new ModdingPageViewModel(mod.View) },
                Tag = id
            };

            var item = new ProjectItem
            {
                Id = id,
                Name = this.ItemName + ".mod",
                FileType = "Mod",
                FilePath = this.ItemName + ".mod"
            };

            this.MainWindowViewModel.Tabs.Add(moddingPage);
            this.MainWindowViewModel.SelectedTabIndex = this.MainWindowViewModel.Tabs.IndexOf(moddingPage);

            if (MainWindowViewModel.SelectedItem == null)
            {
                // Add to root.
                this.MainWindowViewModel.CurrentProject.Items.Add(item);
            }
            else
            {
                ProjectItem selectedItem;
                if (MainWindowViewModel.SelectedItem is TreeViewItem)
                {
                    selectedItem = ((TreeViewItem)this.SelectedItem).Header as ProjectItem;
                }
                else
                {
                    selectedItem = (ProjectItem)MainWindowViewModel.SelectedItem;
                }

                // root
                var i = MainWindowViewModel.CurrentProject.Items.SingleOrDefault(w => w.Id == selectedItem.Id && w.FilePath == selectedItem.FilePath);
                if (i != null)
                {
                    // selected:root/dir1
                    if (i.FileType == "DIRECTORY")
                    {
                        item.FilePath = Path.Combine(i.FilePath, item.FilePath);
                        i.Children.Add(item);
                    }
                    // selected:item1.*
                    else
                        this.MainWindowViewModel.CurrentProject.Items.Add(item);
                }
                else
                {
                    // need fixed path of item.
                    foreach (var innerItem in MainWindowViewModel.CurrentProject.Items)
                    {
                        MainWindowViewModel.RecursiveSearchItem(innerItem, selectedItem, (target, parent) =>
                        {
                            if (target.Id == selectedItem.Id && target.FilePath == selectedItem.FilePath)
                            {
                                if (selectedItem.FileType == "DIRECTORY")
                                {
                                    // selected:root/dir1/dir2
                                    item.FilePath = Path.Combine(target.FilePath, item.FilePath);
                                    target.Children.Add(item);
                                }
                                else
                                {
                                    // selected:root/dir1/item.*
                                    // parent must be directory.
                                    item.FilePath = Path.Combine(Path.GetDirectoryName(target.FilePath), item.FilePath);
                                    parent.Children.Add(item);
                                }
                            }
                        });
                    }
                }


            }
            try
            {
                var data = new ItemData();
                data.Id = Guid.NewGuid().ToString();
                data.Name = item.Name;
                data.PluginId = this.SelectedItem.Id;
                data.PluginVersion = this.SelectedItem.Version;

                var serializer = new DataContractSerializer(typeof(ItemData));
                var writer = XmlWriter.Create(item.FilePath);
                serializer.WriteObject(writer, data);
                writer.Close();
                writer.Dispose();
            }
            catch (Exception e)
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "Error";
                taskDialog.InstructionText = "アイテム追加時にエラーが発生しました。";
                taskDialog.Text = "アイテムを追加する際に、内部エラーが発生したため、処理をキャンセルしました。";
                taskDialog.DetailsCollapsedLabel = "詳細情報を表示する";
                taskDialog.DetailsExpandedText = e.Message;
                taskDialog.DetailsExpandedLabel = "詳細情報を非表示にする";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (_sender, _e) =>
                {
                    ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                };
                taskDialog.Show();
                this.MainWindowViewModel.DeleteItem(item);
            }

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
                this.OKCommand.RaiseCanExecuteChanged();
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
