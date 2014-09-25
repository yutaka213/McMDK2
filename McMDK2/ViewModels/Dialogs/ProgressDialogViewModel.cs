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

namespace McMDK2.ViewModels.Dialogs
{
    public class ProgressDialogViewModel : ViewModel
    {

        public void Initialize()
        {
            this.IsIndeterminate = true;
            this.Value = 100;
        }


        #region IsIndeterminate変更通知プロパティ
        private bool _IsIndeterminate;

        public bool IsIndeterminate
        {
            get
            { return _IsIndeterminate; }
            set
            {
                if (_IsIndeterminate == value)
                    return;
                _IsIndeterminate = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Value変更通知プロパティ
        private int _Value;

        public int Value
        {
            get
            { return _Value; }
            set
            {
                if (_Value == value)
                    return;
                _Value = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Text変更通知プロパティ
        private string _Text;

        public string Text
        {
            get
            { return _Text; }
            set
            {
                if (_Text == value)
                    return;
                _Text = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
