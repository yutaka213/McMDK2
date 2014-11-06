using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

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
using McMDK2.ViewModels.Dialogs;
using McMDK2.Views;
using McMDK2.Views.Dialogs;
using McMDK2.Views.TabPages;
using McMDK2.ViewModels.TabPages;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable AssignNullToNotNullAttribute
namespace McMDK2.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {

        public MainWindowViewModel()
        {
            this.Tabs = new ObservableCollection<TabItem>();
            this.RecentProjects = new ObservableCollection<Project>();
            this.ProjectContextMenuItems = new ObservableCollection<object>();
            this.IsLoadedProject = false;
            this.TaskText = "準備完了";
            this.Title = "Minecraft Mod Development Kit";
            this.SelectedTabIndex = -1;

            if (Define.GetInternalSettings().RecentProjects != null)
            {
                foreach (var item in Define.GetInternalSettings().RecentProjects)
                {
                    if (item != null)
                        this.RecentProjects.Add(item);
                }
            }

            this.InitializeContextMenu();
        }

        public void Initialize()
        {
            var startPage = new TabItem
            {
                Header = "Start",
                Content = new StartPage { DataContext = new StartPageViewModel(this) },
                Tag = Guids.StartPageGuid
            };
            this.Tabs.Add(startPage);
            this.SelectedTabIndex = 0;
        }

        private void InitializeContextMenu()
        {
            var item = new MenuItem();
            item.Header = "追加";
            //item.Click += AddItem;
            {
                var subMenu = new MenuItem();
                subMenu.Header = "新しい項目を追加";
                subMenu.Click += AddItem;
                item.Items.Add(subMenu);

                subMenu = new MenuItem();
                subMenu.Header = "新しいフォルダーを追加";
                subMenu.Click += AddDirectory;
                item.Items.Add(subMenu);
            }
            this.ProjectContextMenuItems.Add(item);

            this.ProjectContextMenuItems.Add(new Separator());

            item = new MenuItem();
            item.Header = "切り取り";
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/Cut_6523.png");
            bitmap.EndInit();
            item.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
            item.Click += CutItem;
            this.ProjectContextMenuItems.Add(item);

            item = new MenuItem();
            item.Header = "コピー";
            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/Copy_6524.png");
            bitmap.EndInit();
            item.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
            item.Click += CopyItem;
            this.ProjectContextMenuItems.Add(item);

            item = new MenuItem();
            item.Header = "貼り付け";
            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/Paste_6520.png");
            bitmap.EndInit();
            item.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
            item.Click += PasteItem;
            this.ProjectContextMenuItems.Add(item);

            item = new MenuItem();
            item.Header = "削除";
            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/action_Cancel_16xLG.png");
            bitmap.EndInit();
            item.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
            item.Click += DeleteItem;
            this.ProjectContextMenuItems.Add(item);

            item = new MenuItem();
            item.Header = "名前を変更";
            item.Click += RenameItem;
            this.ProjectContextMenuItems.Add(item);
        }

        #region Uninitialize
        public void Uninitialize()
        {
            // If loaded project, save it.
            if (this.CurrentProject != null)
            {
                this.CurrentProject.Save();
            }

            var items = new List<Project>();
            // 保存されている個数
            int i = 0;
            if (Define.GetInternalSettings().RecentProjects != null)
                i = Define.GetInternalSettings().RecentProjects.Length;
            // 新しく開かれたプロジェクトの数を算出
            int g = this.RecentProjects.Count - i;
            var rlist = this.RecentProjects.Reverse().ToList();
            // 新しく開かれたプロジェクトの分だけ取り出す
            for (int j = 0; j < g; j++)
            {
                // 重複を除外
                if (items.Where(w => w.Id == rlist[j].Id).ToArray().Length >= 1)
                    continue;
                items.Add(rlist[j]);
            }
            // 保存されている分を追加
            for (int j = 0; j < i; j++)
            {
                // 同じく重複を除外
                if (items.Where(w => w.Id == Define.GetInternalSettings().RecentProjects[j].Id).ToArray().Length >= 1)
                    continue;
                items.Add(Define.GetInternalSettings().RecentProjects[j]);
            }

            var saveItems = new List<Project>();
            int k = items.Count >= 5 ? 5 : items.Count;
            for (int j = 0; j < k; j++)
            {
                // Items の中身をなくす
                items[j].Items.Clear();
                items[j].UserProperties = null;
                saveItems.Add(items[j]);
            }

            Define.GetInternalSettings().RecentProjects = saveItems.ToArray();
            Define.GetInternalSettings().Save();
        }
        #endregion

        // Close Tab.
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
            object content = ((TabItem)parameter).Content;
            if (content != null)
            {
                var view = content as ItemView;
                if (view != null)
                {
                    view.Closing();
                }
                else if (((UserControl)content).DataContext is ItemViewEx)
                {
                    ((ItemViewEx)((UserControl)content).DataContext).Closing();
                    ((ViewModel)((UserControl)content).DataContext).Dispose();
                }
            }
            this.Tabs.Remove((TabItem)parameter);
        }
        #endregion


        #region MouseDoubleClickCommand
        private ListenerCommand<object> _MouseDoubleClickCommand;

        public ListenerCommand<object> MouseDoubleClickCommand
        {
            get
            {
                if (_MouseDoubleClickCommand == null)
                {
                    _MouseDoubleClickCommand = new ListenerCommand<object>(MouseDoubleClick);
                }
                return _MouseDoubleClickCommand;
            }
        }

        // プロジェクトエクスプローラーのItemがWクリック(=選択)された際にコールされます。
        public void MouseDoubleClick(object parameter)
        {
            var item = parameter as ProjectItem;
            this.SelectedItem = item;
            if (item.FileType == Define.IdentifierDirectory)
                return;

            if (this.Tabs.SingleOrDefault(w => (string)w.Tag == item.Id) != null)
            {
                //Open Tab
                this.SelectedTabIndex = this.Tabs.IndexOf(this.Tabs.Single(w => (string)w.Tag == item.Id));
                return;
            }

            var newtab = new TabItem { Header = item.Name, Tag = item.Id };
            var view = ItemManager.GetItemViewFromExtension(Path.GetExtension(item.Name));
            if (view == null)
            {
                // Preview unavailable
                var page = new NullPage();
                var vm = new NullPageViewModel();
                vm.Message = String.Format("この項目({0})ではプレビュー及び編集機能は使用できません。", item.Name);
                page.DataContext = vm;
                newtab.Content = page;
            }
            else
            {
                var itemView = view as ItemView;
                if (itemView != null)
                {
                    itemView.Initialize(item.FilePath);
                }
                else if (view.DataContext != null && view.DataContext is ItemViewEx)
                {
                    ((ItemViewEx)view.DataContext).Initialize(item.FilePath);
                }
                newtab.Content = view;
            }
            this.Tabs.Add(newtab);
            this.SelectedTabIndex = this.Tabs.IndexOf(newtab);
        }
        #endregion


        #region PreviewMouseRightButtonDownCommand
        private ListenerCommand<object> _PreviewMouseRightButtonDownCommand;

        public ListenerCommand<object> PreviewMouseRightButtonDownCommand
        {
            get
            {
                if (_PreviewMouseRightButtonDownCommand == null)
                {
                    _PreviewMouseRightButtonDownCommand = new ListenerCommand<object>(PreviewMouseRightButtonDown);
                }
                return _PreviewMouseRightButtonDownCommand;
            }
        }

        public void PreviewMouseRightButtonDown(object parameter)
        {
            /* parameter[0] is sender, parameter[1] is EventArgs */
            object sender = ((object[])parameter)[0];
            if (sender is TreeViewItem)
            {
                var selectedItem = (ProjectItem)((TreeViewItem)sender).Header;

                #region ContextMenu
                var contextMenu = new ContextMenu();

                var addMenu = new MenuItem();
                addMenu.Header = "追加";
                //addMenu.Click += AddItem;

                var subMenu = new MenuItem();
                subMenu.Header = "新しい項目を追加";
                subMenu.Click += AddItem;
                addMenu.Items.Add(subMenu);

                subMenu = new MenuItem();
                subMenu.Header = "新しいフォルダー";
                subMenu.Click += AddDirectory;
                addMenu.Items.Add(subMenu);

                contextMenu.Items.Add(addMenu);

                var sep = new Separator();
                contextMenu.Items.Add(sep);

                subMenu = new MenuItem();
                subMenu.Header = "切り取り";
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Cut_6523.png");
                bitmap.EndInit();
                subMenu.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
                subMenu.Click += CutItem;
                contextMenu.Items.Add(subMenu);

                subMenu = new MenuItem();
                subMenu.Header = "コピー";
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Copy_6524.png");
                bitmap.EndInit();
                subMenu.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
                subMenu.Click += CopyItem;
                contextMenu.Items.Add(subMenu);

                subMenu = new MenuItem();
                subMenu.Header = "貼り付け";
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Paste_6520.png");
                bitmap.EndInit();
                subMenu.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
                subMenu.Click += PasteItem;
                contextMenu.Items.Add(subMenu);

                subMenu = new MenuItem();
                subMenu.Header = "削除";
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/action_Cancel_16xLG.png");
                bitmap.EndInit();
                subMenu.Icon = new Image { Source = bitmap, Height = 15, Width = 15, UseLayoutRounding = true };
                subMenu.Click += DeleteItem;
                contextMenu.Items.Add(subMenu);

                subMenu = new MenuItem();
                subMenu.Header = "名前を変更";
                subMenu.Click += RenameItem;
                contextMenu.Items.Add(subMenu);

                if (selectedItem.FileType == Define.IdentifierDirectory)
                {
                    sep = new Separator();
                    contextMenu.Items.Add(sep);

                    subMenu = new MenuItem();
                    subMenu.Header = "フォルダーを開く";
                    subMenu.Click += (a, b) =>
                    {
                        string path = ((ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)b.Source).Parent).PlacementTarget).Header).FilePath;
                        if (FileController.Exists(path))
                            Process.Start(path);
                    };
                    contextMenu.Items.Add(subMenu);
                }
                // Generate ContextMenu
                ((TreeViewItem)sender).ContextMenu = contextMenu;

                #endregion

                // Added Event
                ((TreeViewItem)sender).MouseRightButtonDown += MouseRightButtonDown;
            }
        }

        public void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is TreeViewItem)
            {
                ((TreeViewItem)sender).IsSelected = true;
                this.SelectedItem = ((TreeViewItem)sender).Header as ProjectItem;

                // Removed Event
                ((TreeViewItem)sender).MouseRightButtonDown -= MouseRightButtonDown;
                e.Handled = true;
            }
        }

        #region Add a item.
        private void AddItem(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }
            this.SelectedItem = item;
            Messenger.Raise(new TransitionMessage(typeof(NewItemWindow), new NewItemWindowViewModel(this), TransitionMode.Modal, "Transition"));
        }
        #endregion

        #region Add new directory
        private void AddDirectory(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }
            this.SelectedItem = item;

            var vm = new NewDirectoryDialogViewModel();
            Messenger.Raise(new TransitionMessage(typeof(NewDirectoryDialog), vm, TransitionMode.Modal, "Transition"));
            if (String.IsNullOrWhiteSpace(vm.Name))
            {
                return;
            }
            if (this.SelectedItem == null)
            {
                this.CurrentProject.Items.Add(new ProjectItem
                {
                    FilePath = Path.Combine(this.CurrentProject.Path, vm.Name),
                    FileType = Define.IdentifierDirectory,
                    Id = Guids.DirectoryItemGuid,
                    Name = vm.Name
                });
                FileController.CreateDirectory(Path.Combine(this.CurrentProject.Path, vm.Name));

                return;
            }
            this.SearchItem(item, (target, parent) =>
            {
                if (parent == null)
                {
                    if (target.FileType == Define.IdentifierDirectory)
                    {
                        target.Children.Add(new ProjectItem
                        {
                            FilePath = Path.Combine(target.FilePath, vm.Name),
                            FileType = Define.IdentifierDirectory,
                            Id = Guids.DirectoryItemGuid,
                            Name = vm.Name
                        });
                    }
                    else
                    {
                        this.CurrentProject.Items.Add(new ProjectItem
                        {
                            FilePath = Path.Combine(Path.GetDirectoryName(target.FilePath), vm.Name),
                            FileType = Define.IdentifierDirectory,
                            Id = Guids.DirectoryItemGuid,
                            Name = vm.Name
                        });
                    }
                    FileController.CreateDirectory(Path.Combine(target.FileType == Define.IdentifierDirectory ? target.FilePath : Path.GetDirectoryName(target.FilePath), vm.Name));
                }
                else
                {
                    if (target.FileType == Define.IdentifierDirectory)
                    {
                        target.Children.Add(new ProjectItem
                        {
                            FilePath = Path.Combine(target.FilePath, vm.Name),
                            FileType = Define.IdentifierDirectory,
                            Id = Guids.DirectoryItemGuid,
                            Name = vm.Name
                        });
                    }
                    else
                    {
                        parent.Children.Add(new ProjectItem
                        {
                            FilePath = Path.Combine(parent.FilePath, vm.Name),
                            FileType = Define.IdentifierDirectory,
                            Id = Guids.DirectoryItemGuid,
                            Name = vm.Name
                        });
                    }
                    FileController.CreateDirectory(Path.Combine(target.FileType == Define.IdentifierDirectory ? target.FilePath : parent.FilePath, vm.Name));
                }
            });
        }

        #endregion

        #region Cut a selected item.

        private ProjectItem cuttedOrCopiedItem;

        private void CutItem(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }
            if (item != null)
                CutItem(item);
        }

        private void CutItem(ProjectItem item)
        {
            if (this.cuttedOrCopiedItem != null)
            {
                this.SearchItem(this.cuttedOrCopiedItem, (target, parent) =>
                {
                    if (parent == null)
                    {
                        target.IsCut = false;
                        int index = this.CurrentProject.Items.IndexOf(target);
                        this.CurrentProject.Items.RemoveAt(index);
                        this.CurrentProject.Items.Insert(index, target);
                    }
                    else
                    {
                        target.IsCut = false;
                        int index = parent.Children.IndexOf(target);
                        parent.Children.RemoveAt(index);
                        parent.Children.Insert(index, target);
                    }
                });
            }
            this.cuttedOrCopiedItem = item;
            this.SearchItem(this.cuttedOrCopiedItem, (target, parent) =>
            {
                if (parent == null)
                {
                    target.IsCut = true;
                    int index = this.CurrentProject.Items.IndexOf(target);
                    this.CurrentProject.Items.RemoveAt(index);
                    this.CurrentProject.Items.Insert(index, target);
                }
                else
                {
                    target.IsCut = true;
                    int index = parent.Children.IndexOf(target);
                    parent.Children.RemoveAt(index);
                    parent.Children.Insert(index, target);
                }
            });
        }

        #endregion

        #region Copy a selected item.
        private void CopyItem(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }
            if (item != null)
                CopyItem(item);
        }

        private void CopyItem(ProjectItem item)
        {
            // コピー or 切り取り状態のアイテムを、元の状態に戻す。
            if (this.cuttedOrCopiedItem != null)
            {
                this.SearchItem(this.cuttedOrCopiedItem, (target, parent) =>
                {
                    if (parent == null)
                    {
                        target.IsCut = false;
                        int index = this.CurrentProject.Items.IndexOf(target);
                        this.CurrentProject.Items.RemoveAt(index);
                        this.CurrentProject.Items.Insert(index, target);
                    }
                    else
                    {
                        target.IsCut = false;
                        int index = parent.Children.IndexOf(target);
                        parent.Children.RemoveAt(index);
                        parent.Children.Insert(index, target);
                    }
                });
            }
            this.cuttedOrCopiedItem = item;
        }
        #endregion

        #region Paste a selected item.

        private void PasteItem(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }
            if (item != null)
                PasteItem(item);
        }

        /// <summary>
        /// コピーもしくは切り取ったアイテムを貼り付けます。<para />
        /// 親アイテムにディレクトリが設定された場合はその子要素に、<para />
        /// 親アイテムにそれ以外が設定された場合はその親の子要素に追加されます。
        /// </summary>
        /// <param name="item">親となるアイテム</param>
        public void PasteItem(ProjectItem item)
        {
            if (this.cuttedOrCopiedItem == null)
                return;

            this.SearchItem(item, (target, parent) =>
            {
                // root
                if (parent == null)
                {
                    // 単純なファイルの場合
                    if (this.cuttedOrCopiedItem.FileType != Define.IdentifierDirectory)
                    {
                        string srcPath = this.cuttedOrCopiedItem.FilePath;
                        string destPath = Path.Combine(target.FileType == Define.IdentifierDirectory ? target.FilePath : this.CurrentProject.Path, Path.GetFileName(this.cuttedOrCopiedItem.FilePath));
                        if (srcPath == destPath)
                        {
                            MessageBox.Show("同じ場所にファイルを貼り付けることはできません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var copiedItem = (ProjectItem)this.cuttedOrCopiedItem.Clone();
                        copiedItem.FilePath = destPath;
                        copiedItem.Id = Guid.NewGuid().ToString();

                        FileController.Copy(srcPath, destPath);

                        if (copiedItem.IsCut)
                        {
                            copiedItem.IsCut = false;
                            this.SearchItem(this.cuttedOrCopiedItem, (target2, parent2) => this.DeleteItem(target2, false));
                        }

                        if (target.FileType == Define.IdentifierDirectory)
                        {
                            target.Children.Add(copiedItem);
                        }
                        else
                        {
                            this.CurrentProject.Items.Add(copiedItem);
                        }
                    }
                    // Define.IdentifierDirectory
                    else
                    {
                        string srcPath = this.cuttedOrCopiedItem.FilePath;
                        string destPath = Path.Combine(target.FileType == Define.IdentifierDirectory ? target.FilePath : this.CurrentProject.Path, Path.GetFileName(this.cuttedOrCopiedItem.FilePath));
                        if (srcPath == destPath)
                        {
                            MessageBox.Show("同じ場所にファイルを貼り付けることはできません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var copiedItem = (ProjectItem)this.cuttedOrCopiedItem.Clone();
                        this.ActionItem(copiedItem, (target2, parent2) =>
                        {
                            target2.FilePath = target2.FilePath.Replace(this.cuttedOrCopiedItem.FilePath, destPath);
                            if (target2.FileType != Guids.DirectoryItemGuid)
                                target2.Id = Guid.NewGuid().ToString();
                        });
                        FileController.Copy(srcPath, Path.Combine(target.FileType == Define.IdentifierDirectory ? target.FilePath : this.CurrentProject.Path, copiedItem.Name));

                        if (copiedItem.IsCut)
                        {
                            copiedItem.IsCut = false;
                            this.SearchItem(this.cuttedOrCopiedItem, (target2, parent2) => this.DeleteItem(target2, false));
                        }

                        if (target.FileType == Define.IdentifierDirectory)
                        {
                            target.Children.Add(copiedItem);
                        }
                        else
                        {
                            this.CurrentProject.Items.Add(copiedItem);
                        }
                    }
                }
                else
                {
                    // 単純なファイルの場合
                    if (this.cuttedOrCopiedItem.FileType != Define.IdentifierDirectory)
                    {
                        string srcPath = this.cuttedOrCopiedItem.FilePath;
                        string destPath = Path.Combine(parent.FilePath, Path.GetFileName(this.cuttedOrCopiedItem.FilePath));

                        if (srcPath == destPath)
                        {
                            MessageBox.Show("同じ場所にファイルを貼り付けることはできません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var copiedItem = (ProjectItem)this.cuttedOrCopiedItem.Clone();
                        copiedItem.FilePath = destPath;
                        copiedItem.Id = Guid.NewGuid().ToString();

                        FileController.Copy(srcPath, destPath);

                        if (copiedItem.IsCut)
                        {
                            copiedItem.IsCut = false;
                            this.SearchItem(this.cuttedOrCopiedItem, (target2, parent2) => this.DeleteItem(target2, false));
                        }
                        parent.Children.Add(copiedItem);
                    }
                    // Define.IdentifierDirectory
                    else
                    {
                        string srcPath = this.cuttedOrCopiedItem.FilePath;
                        string destPath = Path.Combine(parent.FilePath, Path.GetFileName(this.cuttedOrCopiedItem.FilePath));
                        if (srcPath == destPath)
                        {
                            MessageBox.Show("同じ場所にファイルを貼り付けることはできません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var copiedItem = (ProjectItem)this.cuttedOrCopiedItem.Clone();
                        this.ActionItem(copiedItem, (target2, parent2) =>
                        {
                            target2.FilePath = target2.FilePath.Replace(this.cuttedOrCopiedItem.FilePath, destPath);
                            if (target2.FileType != Guids.DirectoryItemGuid)
                                target2.Id = Guid.NewGuid().ToString();
                        });
                        FileController.Copy(srcPath, destPath);

                        if (copiedItem.IsCut)
                        {
                            copiedItem.IsCut = false;
                            this.SearchItem(this.cuttedOrCopiedItem, (target2, parent2) => this.DeleteItem(target2, false));
                        }
                        parent.Children.Add(copiedItem);
                    }
                }
            });
        }

        #endregion

        #region Delete a selected item.

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }
            if (item != null)
                DeleteItem(item);
        }

        public void DeleteItem(ProjectItem item, bool openDialog = true)
        {
            if (FileController.Exists(item.FilePath))
            {
                if (openDialog && MessageBox.Show("ファイルを削除しますか？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
            }
            var tab = this.Tabs.SingleOrDefault(w => (string)w.Tag == item.Id);
            if (tab != null)
                this.Tabs.Remove(tab);

            this.SearchItem(item, (target, parent) =>
            {
                if (parent == null)
                {
                    this.CurrentProject.Items.Remove(target);
                    FileController.Delete(target.FilePath);
                    this.SelectedItem = null;
                }
                else
                {
                    parent.Children.Remove(item);
                    FileController.Delete(item.FilePath);
                    if (parent.Children.Count == 0)
                    {
                        parent.Id = Guids.DirectoryItemGuid;
                    }
                    this.SelectedItem = null;
                }
            });
        }
        #endregion

        #region Rename a selected item.
        private void RenameItem(object sender, RoutedEventArgs e)
        {
            ProjectItem item = null;
            if (sender is TreeViewItem)
            {
                item = (ProjectItem)((TreeViewItem)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Header;
            }
            else
            {
                if (this.SelectedItem != null)
                {
                    if (this.SelectedItem is TreeViewItem)
                        this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                    item = this.SelectedItem as ProjectItem;
                }
            }

            if (item != null)
            {
                var vm = new RenameDialogViewModel();
                vm.ToName = item.Name;
                // Input New Name
                Messenger.Raise(new TransitionMessage(typeof(RenameDialog), vm, TransitionMode.Modal, "Transition"));
                this.RenameItem(item, vm.ToName);
            }
        }

        public void RenameItem(ProjectItem item, string newName)
        {
            if (String.IsNullOrWhiteSpace(newName) || item.Name == newName)
            {
                return;
            }

            this.SearchItem(item, (target, parent) =>
            {
                if (parent == null)
                {
                    string oldPath = target.FilePath;

                    item.Name = newName;
                    item.FilePath = Path.Combine(Path.GetDirectoryName(item.FilePath), newName);
                    this.CurrentProject.Items.Remove(target);
                    this.CurrentProject.Items.Add(item);

                    FileController.Rename(oldPath, item.FilePath);
                }
                else
                {
                    string oldPath = target.FilePath;
                    item.Name = newName;
                    item.FilePath = Path.Combine(Path.GetDirectoryName(item.FilePath), newName);
                    parent.Children.Remove(target);
                    parent.Children.Add(item);

                    FileController.Rename(oldPath, item.FilePath);
                }
            });
        }
        #endregion

        #endregion


        // ##############################################################
        // File(_F)
        // ##############################################################
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
            Messenger.Raise(new TransitionMessage(typeof(NewWizardWindow), new NewWizardWindowViewModel(this), TransitionMode.Modal, "Transition"));
        }
        #endregion


        #region OpenProjectCommand
        private ViewModelCommand _OpenProjectCommand;

        public ViewModelCommand OpenProjectCommand
        {
            get
            {
                if (_OpenProjectCommand == null)
                {
                    _OpenProjectCommand = new ViewModelCommand(OpenProject);
                }
                return _OpenProjectCommand;
            }
        }

        public void OpenProject()
        {
            this.OpenProject("");
        }

        public void OpenProject(string openPath)
        {
            if (String.IsNullOrEmpty(openPath))
            {
                var ofd = new OpenFileDialog
                {
                    FileName = "",
                    Filter = "McMDK Main Project File (*.mdk)|*.mdk",
                    InitialDirectory = String.IsNullOrEmpty(Define.GetInternalSettings().LastOpenedDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) : Define.GetInternalSettings().LastOpenedDirectory,
                };
                if (ofd.ShowDialog() == true)
                {
                    openPath = ofd.FileName;
                    Define.GetInternalSettings().LastOpenedDirectory = Path.GetDirectoryName(openPath);
                }
            }
            if (!String.IsNullOrEmpty(openPath))
            {
                if (FileController.Exists(openPath))
                {
                    var progress = new ProgressDialogViewModel();
                    progress.SetIndeterminate(true);
                    progress.SetText("プロジェクトを読み込んでいます...");
                    progress.Action += () =>
                    {
                        string file = openPath;
                        string json;
                        using (var sr = new StreamReader(file))
                        {
                            json = sr.ReadToEnd();
                        }
                        var obj = JsonConvert.DeserializeObject<Project>(json);
                        obj.Items = new ObservableCollection<ProjectItem>(); // Items is always clear.
                        this.Title = obj.Name + " - Minecraft Mod Development Kit";
                        string rootpath = obj.Path + "\\";

                        // Loading MINECRAFT MOD PROJECT(*.mmproj) file.
                        var element = XElement.Load(Path.Combine(obj.Path, obj.Name + ".mmproj"));
                        var q = from p in element.Element("Items").Elements()
                                select new
                                {
                                    Include = p.Attribute("Include").Value,
                                    Id = p.Attribute("Id") == null ? Guid.NewGuid().ToString() : p.Attribute("Id").Value
                                };

                        foreach (var item in q)
                        {
                            string[] path = item.Include.Split('\\');
                            ObservableCollection<ProjectItem> cur = obj.Items;
                            for (int i = 0; i < path.Length; i++)
                            {
                                // Include="ITEM.EXT"
                                if (path.Length - 1 == 0)
                                {
                                    obj.Items.Add(new ProjectItem
                                    {
                                        Name = path[0],
                                        FileType = item.Id == Guids.DirectoryItemGuid ? Define.IdentifierDirectory : ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[0])),
                                        FilePath = rootpath + path[0],
                                        Id = item.Id
                                    });
                                }
                                // Include="DIR/ITEM.EXT"
                                else
                                {
                                    // DIR
                                    if (i != path.Length - 1)
                                    {
                                        var sb = new StringBuilder();
                                        for (int j = 0; j < i + 1; j++)
                                        {
                                            sb.Append(path[j]);
                                            sb.Append("\\");
                                        }
                                        sb.Remove(sb.Length - 1, 1);

                                        if (cur.SingleOrDefault(w => w.FilePath == rootpath + sb.ToString()) == null)
                                        {
                                            cur.Add(new ProjectItem
                                            {
                                                Name = path[i],
                                                FileType = Define.IdentifierDirectory,
                                                FilePath = rootpath + sb
                                            });
                                        }
                                        cur = cur.Single(w => w.FilePath == rootpath + sb.ToString()).Children;
                                    }
                                    // ITEM.EXT
                                    else
                                    {
                                        cur.Add(new ProjectItem
                                        {
                                            Name = path[i],
                                            FileType =
                                                item.Id == Guids.DirectoryItemGuid ? Define.IdentifierDirectory : ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[i])),
                                            FilePath = rootpath + item.Include,
                                            Id = item.Id
                                        });
                                    }
                                }
                            }
                        }

                        this.CurrentProject = obj;
                        progress.Close();

                        DispatcherHelper.UIDispatcher.Invoke(() =>
                        {
                            var tab = this.Tabs.SingleOrDefault(w => (string)w.Tag == Guids.StartPageGuid);
                            if (tab != null)
                                this.Tabs.Remove(tab);

                            this.RecentProjects.Add(obj);
                            this.IsLoadedProject = true;
                        });

                    };
                    Messenger.Raise(new TransitionMessage(typeof(ProgressDialog), progress, TransitionMode.Modal, "Transition"));

                }
                else
                {
                    MessageBox.Show("ファイルを開けませんでした。");
                }
            }
        }
        #endregion


        #region CloseTabCommand
        private ViewModelCommand _CloseTabCommand;

        public ViewModelCommand CloseTabCommand
        {
            get
            {
                if (_CloseTabCommand == null)
                {
                    _CloseTabCommand = new ViewModelCommand(CloseTab);
                }
                return _CloseTabCommand;
            }
        }

        public void CloseTab()
        {
            if (this.Tabs.Count > this.SelectedTabIndex && this.SelectedTabIndex >= 0)
                this.Tabs.RemoveAt(this.SelectedTabIndex);
        }
        #endregion


        #region CloseProjectCommand
        private ViewModelCommand _CloseProjectCommand;

        public ViewModelCommand CloseProjectCommand
        {
            get
            {
                if (_CloseProjectCommand == null)
                {
                    _CloseProjectCommand = new ViewModelCommand(CloseProject);
                }
                return _CloseProjectCommand;
            }
        }

        public void CloseProject()
        {
            this.Tabs.Clear();
            this.IsLoadedProject = false;
            if (this.CurrentProject != null)
            {
                this.CurrentProject.Save();
                this.CurrentProject = null;
                this.Title = "Minecraft Mod Development Kit";
            }
        }
        #endregion


        #region SaveProjectCommand
        private ViewModelCommand _SaveProjectCommand;

        public ViewModelCommand SaveProjectCommand
        {
            get
            {
                if (_SaveProjectCommand == null)
                {
                    _SaveProjectCommand = new ViewModelCommand(SaveProject);
                }
                return _SaveProjectCommand;
            }
        }

        public void SaveProject()
        {
            if (this.CurrentProject != null)
            {
                this.CurrentProject.Save();
            }
        }
        #endregion


        #region CloseAppCommand
        private ViewModelCommand _CloseAppCommand;

        public ViewModelCommand CloseAppCommand
        {
            get
            {
                if (_CloseAppCommand == null)
                {
                    _CloseAppCommand = new ViewModelCommand(CloseApp);
                }
                return _CloseAppCommand;
            }
        }

        public void CloseApp()
        {
            Environment.Exit(0);
        }
        #endregion


        // ##############################################################
        // Edit(_E)
        // ##############################################################
        #region CutItemCommand
        private ViewModelCommand _CutItemCommand;

        public ViewModelCommand CutItemCommand
        {
            get
            {
                if (_CutItemCommand == null)
                {
                    _CutItemCommand = new ViewModelCommand(CutItem);
                }
                return _CutItemCommand;
            }
        }

        public void CutItem()
        {
            if (this.SelectedItem != null)
            {
                if (this.SelectedItem is TreeViewItem)
                    this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                this.CutItem(this.SelectedItem as ProjectItem);
            }
        }
        #endregion


        #region CopyItemCommand
        private ViewModelCommand _CopyItemCommand;

        public ViewModelCommand CopyItemCommand
        {
            get
            {
                if (_CopyItemCommand == null)
                {
                    _CopyItemCommand = new ViewModelCommand(CopyItem);
                }
                return _CopyItemCommand;
            }
        }

        public void CopyItem()
        {
            if (this.SelectedItem != null)
            {
                if (this.SelectedItem is TreeViewItem)
                    this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                this.CopyItem(this.SelectedItem as ProjectItem);
            }
        }
        #endregion


        #region PasteItemCommand
        private ViewModelCommand _PasteItemCommand;

        public ViewModelCommand PasteItemCommand
        {
            get
            {
                if (_PasteItemCommand == null)
                {
                    _PasteItemCommand = new ViewModelCommand(PasteItem);
                }
                return _PasteItemCommand;
            }
        }

        public void PasteItem()
        {
            if (this.SelectedItem != null)
            {
                if (this.SelectedItem is TreeViewItem)
                    this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                this.PasteItem(this.SelectedItem as ProjectItem);
            }
        }
        #endregion


        #region DeleteItemCommand
        private ViewModelCommand _DeleteItemCommand;

        public ViewModelCommand DeleteItemCommand
        {
            get
            {
                if (_DeleteItemCommand == null)
                {
                    _DeleteItemCommand = new ViewModelCommand(DeleteItem);
                }
                return _DeleteItemCommand;
            }
        }

        public void DeleteItem()
        {
            if (this.SelectedItem != null)
            {
                if (this.SelectedItem is TreeViewItem)
                    this.SelectedItem = ((TreeViewItem)this.SelectedItem).Header;
                this.DeleteItem(this.SelectedItem as ProjectItem);
            }
        }
        #endregion


        // ##############################################################
        // View(_V)
        // ##############################################################

        // TODO: Implementation

        // ##############################################################
        // Project(_P)
        // ##############################################################
        #region AddNewItemCommand
        private ViewModelCommand _AddNewItemCommand;

        public ViewModelCommand AddNewItemCommand
        {
            get
            {
                if (_AddNewItemCommand == null)
                {
                    _AddNewItemCommand = new ViewModelCommand(AddNewItem);
                }
                return _AddNewItemCommand;
            }
        }

        public void AddNewItem()
        {
            Messenger.Raise(new TransitionMessage(typeof(NewItemWindow), new NewItemWindowViewModel(this), TransitionMode.Modal, "Transition"));
        }
        #endregion


        #region ProjectInfoCommand
        private ViewModelCommand _ProjectInfoCommand;

        public ViewModelCommand ProjectInfoCommand
        {
            get
            {
                if (_ProjectInfoCommand == null)
                {
                    _ProjectInfoCommand = new ViewModelCommand(ProjectInfo);
                }
                return _ProjectInfoCommand;
            }
        }

        public void ProjectInfo()
        {
            var projectInfoPage = new TabItem
            {
                Header = "プロジェクト設定",
                Content = new ProjectSettingPage { DataContext = new ProjectSettingPageViewModel() },
                Tag = Guids.ProjectInfoPageGuid
            };
            this.Tabs.Add(projectInfoPage);
            this.SelectedTabIndex = this.Tabs.IndexOf(projectInfoPage);
        }
        #endregion


        // ##############################################################
        // Tool(_T)
        // ##############################################################

        // TODO: Implementation

        // ##############################################################
        // Help(_H)
        // ##############################################################
        #region ShowAboutDialogCommand
        private ViewModelCommand _ShowAboutDialogCommand;

        public ViewModelCommand ShowAboutDialogCommand
        {
            get
            {
                if (_ShowAboutDialogCommand == null)
                {
                    _ShowAboutDialogCommand = new ViewModelCommand(ShowAboutDialog);
                }
                return _ShowAboutDialogCommand;
            }
        }

        public void ShowAboutDialog()
        {
            Messenger.Raise(new TransitionMessage(typeof(AboutDialog), new AboutDialogViewModel(), TransitionMode.Modal, "Transition"));
        }
        #endregion

        /// <summary>
        /// 対象のアイテムを検索し、アイテムが存在した場合、actionを実行します。
        /// </summary>
        /// <param name="targetItem">検索するアイテム</param>
        /// <param name="action">存在時、実行する動作(対象のアイテム, 親アイテム)</param>
        public void SearchItem(ProjectItem targetItem, Action<ProjectItem, ProjectItem> action)
        {
            var i = this.CurrentProject.Items.SingleOrDefault(w => w.Id == targetItem.Id && w.FilePath == targetItem.FilePath);
            if (i != null)
            {
                action(i, null);
            }
            foreach (var innerItem in this.CurrentProject.Items)
            {
                RecursiveSearchItem(innerItem, targetItem, action);
            }
        }

        private void RecursiveSearchItem(ProjectItem item, ProjectItem targetItem, Action<ProjectItem, ProjectItem> action)
        {
            foreach (var innerItem in item.Children)
            {
                if (innerItem.Id == targetItem.Id)
                {
                    if (innerItem.FilePath == targetItem.FilePath)
                    {
                        action(innerItem, item);
                        break;
                    }
                }
                RecursiveSearchItem(innerItem, targetItem, action);
            }
        }

        /// <summary>
        /// 対象アイテムの子を含む全ての要素に対して、actionを実行します。
        /// </summary>
        /// <param name="targetItem">検索するアイテム</param>
        /// <param name="action">実行する動作(対象のアイテム,親アイテム)</param>
        public void ActionItem(ProjectItem targetItem, Action<ProjectItem, ProjectItem> action)
        {
            action(targetItem, null);
            foreach (var item in targetItem.Children)
            {
                ActionItem(item, action);
            }
        }


        #region CurrentProject変更通知プロパティ
        private Project _CurrentProject;

        public Project CurrentProject
        {
            get
            { return _CurrentProject; }
            set
            {
                if (_CurrentProject == value)
                    return;
                _CurrentProject = value;
                RaisePropertyChanged();
            }
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


        #region IsLoadedProject変更通知プロパティ
        private bool _IsLoadedProject;

        public bool IsLoadedProject
        {
            get
            { return _IsLoadedProject; }
            set
            {
                if (_IsLoadedProject == value)
                    return;
                _IsLoadedProject = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region TaskText変更通知プロパティ
        private string _TaskText;

        public string TaskText
        {
            get
            { return _TaskText; }
            set
            {
                if (_TaskText == value)
                    return;
                _TaskText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region RecentProjects変更通知プロパティ
        private ObservableCollection<Project> _RecentProjects;

        public ObservableCollection<Project> RecentProjects
        {
            get
            { return _RecentProjects; }
            set
            {
                if (_RecentProjects == value)
                    return;
                _RecentProjects = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectContextMenuItems変更通知プロパティ
        private ObservableCollection<object> _ProjectContextMenuItems;

        public ObservableCollection<object> ProjectContextMenuItems
        {
            get
            { return _ProjectContextMenuItems; }
            set
            {
                if (_ProjectContextMenuItems == value)
                    return;
                _ProjectContextMenuItems = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedTabIndex変更通知プロパティ
        private int _SelectedTabIndex;

        public int SelectedTabIndex
        {
            get
            { return _SelectedTabIndex; }
            set
            {
                if (_SelectedTabIndex == value)
                    return;
                _SelectedTabIndex = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get
            { return _Title; }
            set
            {
                if (_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItemプロパティ
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
            }
        }
        #endregion

    }
}
