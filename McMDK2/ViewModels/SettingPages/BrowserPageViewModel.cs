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
    public class BrowserPageViewModel : ViewModel, IConfiguration
    {
        public void Load()
        {
            this.BrowserPath = Define.GetSettings().BrowserFilePath;
        }

        public void Apply()
        {
            Define.GetSettings().BrowserFilePath = this.BrowserPath;
        }


        #region BrowserPath変更通知プロパティ
        private string _BrowserPath;

        public string BrowserPath
        {
            get
            { return _BrowserPath; }
            set
            {
                if (_BrowserPath == value)
                    return;
                _BrowserPath = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
