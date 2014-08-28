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
using McMDK2.Core.Plugin;
using McMDK2.Models;

namespace McMDK2.ViewModels
{
    public class NewWizardWindowViewModel : ViewModel
    {
        public NewWizardWindowViewModel()
        {
            this.ProjectTemplates = new ObservableCollection<ITemplate>(TemplateManager.Templates);
        }

        public void Initialize()
        {

        }


        #region ProjectTemplates変更通知プロパティ
        private ObservableCollection<ITemplate> _ProjectTemplates;

        public ObservableCollection<ITemplate> ProjectTemplates
        {
            get
            { return _ProjectTemplates; }
            set
            {
                if (_ProjectTemplates == value)
                    return;
                _ProjectTemplates = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
