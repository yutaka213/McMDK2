using System;
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
using McMDK2.Core;
using McMDK2.Models;
using McMDK2.Plugin;

namespace McMDK2.ViewModels.SettingPages
{
    public class JavaPageViewModel : ViewModel, IConfiguration
    {
        public void Load()
        {
            this.JavawFilePath = Define.GetSettings().JavawFilePath;
            this.JvmArguments = Define.GetSettings().JVMArguments;
        }

        public void Apply()
        {
            Define.GetSettings().JavawFilePath = this.JavawFilePath;
            Define.GetSettings().JVMArguments = this.JvmArguments;
        }


        #region JavawFilePath変更通知プロパティ
        private string _JavawFilePath;

        public string JavawFilePath
        {
            get
            { return _JavawFilePath; }
            set
            {
                if (_JavawFilePath == value)
                    return;
                _JavawFilePath = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region JvmArguments変更通知プロパティ
        private string _JvmArguments;

        public string JvmArguments
        {
            get
            { return _JvmArguments; }
            set
            {
                if (_JvmArguments == value)
                    return;
                _JvmArguments = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
