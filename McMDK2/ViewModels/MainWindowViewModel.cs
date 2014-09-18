using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
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
using McMDK2.Views.TabPages;
using McMDK2.ViewModels.TabPages;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McMDK2.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            this.Tabs = new ObservableCollection<TabItem>();
            this.IsLoadedProject = false;
            this.TaskText = "準備完了";
        }

        public void Initialize()
        {
            TabItem startPage = new TabItem { Header = "Start" };
            startPage.Content = new StartPage() { DataContext = new StartPageViewModel(this) };
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
            Messenger.Raise(new TransitionMessage(new NewWizardWindowViewModel(this), "ShowNewWizard"));
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "McMDK Main Project File (*.mdk)|*.mdk";
            if (ofd.ShowDialog() == true)
            {
                if (FileController.Exists(ofd.FileName))
                {
                    string file = ofd.FileName;
                    string json = "";
                    using (var sr = new StreamReader(file))
                    {
                        json = sr.ReadToEnd();
                    }
                    var obj = JsonConvert.DeserializeObject<Project>(json);
                    obj.Items = new ObservableCollection<ProjectItem>(); // Items is always clear.
                    string rootpath = obj.Path + "\\";

                    // Loadin MINECRAFT MOD PROJECT(*.mmproj) file.
                    var element = XElement.Load(obj.Path + "//" + obj.Name + ".mmproj");
                    var q = from p in element.Element("Items").Elements()
                            select new
                            {
                                Include = p.Attribute("Include").Value
                            };

                    foreach (var item in q)
                    {
                        string[] path = item.Include.Split('\\');
                        ObservableCollection<ProjectItem> cur = obj.Items;
                        for (int i = 0; i < path.Length; i++)
                        {
                            if (path.Length - 1 == 0)
                            {
                                obj.Items.Add(new ProjectItem
                                {
                                    Name = path[0],
                                    FileType = ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[0])),
                                    FilePath = rootpath + item.Include
                                });
                            }
                            else
                            {
                                if (i != path.Length - 1)
                                {
                                    if (cur.SingleOrDefault(w => w.Name == path[i]) == null)
                                    {
                                        cur.Add(new ProjectItem
                                        {
                                            Name = path[i],
                                            FileType = "DIRECTORY",
                                            FilePath = rootpath + item.Include
                                        });
                                    }
                                    cur = cur.Single(w => w.Name == path[i]).Children;
                                }
                                else
                                {
                                    cur.Add(new ProjectItem
                                    {
                                        Name = path[i],
                                        FileType = ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[i])),
                                        FilePath = rootpath + item.Include
                                    });
                                }
                            }
                        }
                    }

                    this.CurrentProject = obj;
                    var tab = this.Tabs.SingleOrDefault(w => (string)w.Header == "Start");
                    if (tab != null)
                        this.Tabs.Remove(tab);
                }
                else
                {
                    System.Windows.MessageBox.Show("ファイルを開けませんでした。");
                }
            }
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

        public void MouseDoubleClick(object parameter)
        {
            var item = parameter as ProjectItem;
            if (item.FileType == "DIRECTORY")
                return;
            System.Windows.MessageBox.Show(item.FilePath);
            //var view = ItemManager.GetItemViewFromExtension(Path.GetExtension(item.Name));
        }
        #endregion


        #region CurrentProject変更通知プロパティ
        private McMDK2.Core.Data.Project _CurrentProject;

        public McMDK2.Core.Data.Project CurrentProject
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

    }
}
