using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Models;
using McMDK2.Core.Data.Project;
using McMDK2.Core.Data.Project.Internal;

namespace McMDK2.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {

        public void Initialize()
        {
            this.CurrentProject = new ModProject();
            this.CurrentProject.Name = "TestProject";

            this.CurrentProject.Items.Add(new ModItem
            {
                UniqueId = "Hoge",
                Children = new ObservableCollection<Item>
                {
                    new ModItem{UniqueId = "Fuga"},
                    new ModItem{UniqueId = "Foo"}
                }
            });
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



        #region CurrentProject変更通知プロパティ
        private ModProject _CurrentProject;

        public ModProject CurrentProject
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

    }
}
