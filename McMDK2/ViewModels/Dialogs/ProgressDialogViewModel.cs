using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using Livet;
using Livet.Behaviors.Messaging;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Models;

namespace McMDK2.ViewModels.Dialogs
{
    public delegate void DoAction();

    public class ProgressDialogViewModel : ViewModel
    {
        public event DoAction Action;

        public ProgressDialogViewModel()
        {
            this.IsIndeterminate = true;
            this.Value = 0;
        }

        public void Initialize()
        {
            DoAction act = Action;
            if (act != null)
            {
                Action();
            }
        }

        public void SetIndeterminate(bool i)
        {
            this.IsIndeterminate = i;
        }

        public void SetValue(int v)
        {
            this.Value = v;
        }

        public void SetText(string t)
        {
            this.Text = t;
        }

        public async void Close()
        {
            await Messenger.RaiseAsync(new WindowActionMessage(WindowAction.Close, "WindowAction"));
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
