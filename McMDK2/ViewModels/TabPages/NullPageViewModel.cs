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

using McMDK2.Models;

namespace McMDK2.ViewModels.TabPages
{
    public class NullPageViewModel : ViewModel
    {
        public void Initialize()
        {
            this.Message = "この項目ではプレビュー及び編集機能は使用できません。";
        }


        #region Message変更通知プロパティ
        private string _Message;

        public string Message
        {
            get
            { return _Message; }
            set
            {
                if (_Message == value)
                    return;
                _Message = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
