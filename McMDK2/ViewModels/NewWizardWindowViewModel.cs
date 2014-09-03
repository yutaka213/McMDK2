using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Plugin;
using McMDK2.Core;
using McMDK2.Core.Plugin;
using McMDK2.Models;

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
