using System;
using System.Collections.Generic;
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

using McMDK2.Models;
using McMDK2.Plugin;

namespace McMDK2.ViewModels.TabPages
{
    public class ModdingPageViewModel : ViewModel, ItemViewEx
    {
        public void Initialize(string path)
        {
            if (this.ModdingContent != null)
            {

            }
        }

        public void Closing()
        {
            if (this.ModdingContent != null)
            {
                this.ModdingContent.Closing();
            }
        }


        #region ModdingContent変更通知プロパティ
        private /*UserControl*/ModView _ModdingContent;

        public /*UserControl*/ModView ModdingContent
        {
            get
            { return _ModdingContent; }
            set
            {
                if (_ModdingContent == value)
                    return;
                _ModdingContent = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
