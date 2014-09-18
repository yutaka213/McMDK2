using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Plugin;
using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Plugin;
using McMDK2.Models;

using Newtonsoft.Json;

namespace McMDK2.ViewModels
{
    public class NewWizardWindowViewModel : ViewModel
    {
        private MainWindowViewModel MainWindowViewModel;

        public NewWizardWindowViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.Templates = new ObservableCollection<ITemplate>(TemplateManager.Templates);
            this.ProjectPath = Define.ProjectsDirectory;
        }

        public void Initialize()
        {

        }


        #region ProjectTemplates変更通知プロパティ
        private ObservableCollection<ITemplate> _Templates;

        public ObservableCollection<ITemplate> Templates
        {
            get
            { return _Templates; }
            set
            {
                if (_Templates == value)
                    return;
                _Templates = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItem変更通知プロパティ
        private ITemplate _SelectedItem;

        public ITemplate SelectedItem
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


        #region ProjectName変更通知プロパティ
        private string _ProjectName;

        public string ProjectName
        {
            get
            { return _ProjectName; }
            set
            {
                if (_ProjectName == value)
                    return;
                _ProjectName = value;
                var projectPath = Define.ProjectsDirectory + "\\" + _ProjectName;
                int i = 1;
                while (FileController.Exists(projectPath))
                {
                    projectPath = Define.ProjectsDirectory + "\\" + _ProjectName + (i++).ToString();
                }
                this.ProjectPath = projectPath;
                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region ProjectPath変更通知プロパティ
        private string _ProjectPath;

        public string ProjectPath
        {
            get
            { return _ProjectPath; }
            set
            {
                if (_ProjectPath == value)
                    return;
                _ProjectPath = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectVersion変更通知プロパティ
        private string _ProjectVersion;

        public string ProjectVersion
        {
            get
            { return _ProjectVersion; }
            set
            {
                if (_ProjectVersion == value)
                    return;
                _ProjectVersion = value;
                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


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
            var newProject = new Project
            {
                Version = "1.0.0",
                Path = this.ProjectPath,
                Name = this.ProjectName,
                Items = new ObservableCollection<ProjectItem>(),
                Id = Guid.NewGuid().ToString()
            };

            FileController.CreateDirectory(newProject.Path);

            var template = this.SelectedItem;
            string root = "";
            Stream stream;
            if (template.TemplateFile.Split(';').Length == 2)
            {
                string path = template.TemplateFile.Split(';')[1];
                stream = Assembly.GetAssembly(template.GetType()).GetManifestResourceStream(path);
                root = path.Replace(".zip", "").Split('.')[path.Replace(".zip", "").Split('.').Length - 1];
            }
            else
            {
                stream = new FileStream(template.TemplateFile, FileMode.Open, FileAccess.Read);
                root = Path.GetFileNameWithoutExtension(template.TemplateFile);
            }
            FileStream fs = new FileStream(newProject.Path + "//Template.zip", FileMode.Create, FileAccess.Write);
            int nbyte;
            while ((nbyte = stream.ReadByte()) != -1)
            {
                fs.WriteByte((byte)nbyte);
            }
            fs.Close();
            stream.Close();

            ZipFile.ExtractToDirectory(newProject.Path + "//Template.zip", newProject.Path);
            FileController.Delete(newProject.Path + "//Template.zip");
            FileController.Rename(newProject.Path + "//" + root + ".mmproj", newProject.Path + "//" + newProject.Name + ".mmproj");

            var json = JsonConvert.SerializeObject(newProject, Newtonsoft.Json.Formatting.Indented);
            using (var sw = new StreamWriter(newProject.Path + "//project.mdk") /* JSON FORMAT */)
            {
                sw.WriteLine(json);
            }

            string rootpath = newProject.Path + "\\";
            // Loading MINECRAFT MOD PROJECT(*.MMPROJ) file.
            var element = XElement.Load(newProject.Path + "//" + newProject.Name + ".mmproj");
            var q = from p in element.Element("Items").Elements()
                    select new
                    {
                        Include = p.Attribute("Include").Value
                    };

            foreach (var item in q)
            {
                string[] path = item.Include.Split('\\');
                ObservableCollection<ProjectItem> cur = newProject.Items;
                for (int i = 0; i < path.Length; i++)
                {
                    if (path.Length - 1 == 0)
                    {
                        newProject.Items.Add(new ProjectItem
                        {
                            Name = path[0],
                            FileType = ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[0])),
                            FilePath = rootpath + item.Include
                        });
                    }
                    else
                    {
                        // do not FILE
                        if (i != path.Length - 1)
                        {
                            // nf directory
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
                        // FILE
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

            this.MainWindowViewModel.IsLoadedProject = true;
            this.MainWindowViewModel.CurrentProject = newProject;
            this.MainWindowViewModel.RecentProjects.Add(newProject);

            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));

            var tab = this.MainWindowViewModel.Tabs.SingleOrDefault(w => (string)w.Header /* Suppress warning CS0253 */ == "Start");
            if (tab != null)
                this.MainWindowViewModel.Tabs.Remove(tab);
        }

        public bool CanOK()
        {
            if (String.IsNullOrWhiteSpace(this.ProjectName) || String.IsNullOrWhiteSpace(this.ProjectVersion))
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
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }
        #endregion

    }
}
