using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core;
using McMDK2.Models;
using McMDK2.Views.Dialogs;

namespace McMDK2.ViewModels.Dialogs
{
    public class AboutDialogViewModel : ViewModel
    {
        public void Initialize()
        {
            this.Version = Define.Version;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (Define.FoundNewVersion)
            {
                this.UpdateStatusText = "Update Available";
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/StatusAnnotations_Information_16xLG_color.png");
            }
            else
            {
                this.UpdateStatusText = "Latest Version";
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/StatusAnnotations_Complete_and_ok_16xLG_color.png");
            }
            bitmap.EndInit();
            this.UpdateStatus = bitmap;
        }


        #region ShowPluginInfoCommand
        private ViewModelCommand _ShowPluginInfoCommand;

        public ViewModelCommand ShowPluginInfoCommand
        {
            get
            {
                if (_ShowPluginInfoCommand == null)
                {
                    _ShowPluginInfoCommand = new ViewModelCommand(ShowPluginInfo);
                }
                return _ShowPluginInfoCommand;
            }
        }

        public void ShowPluginInfo()
        {
            Messenger.Raise(new TransitionMessage(typeof(PluginInfoDialog), new PluginInfoDialogViewModel(), TransitionMode.Modal, "Transition"));
        }
        #endregion



        #region Version変更通知プロパティ
        private string _Version;

        public string Version
        {
            get
            { return _Version; }
            set
            {
                if (_Version == value)
                    return;
                _Version = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region UpdateStatus変更通知プロパティ
        private BitmapImage _UpdateStatus;

        public BitmapImage UpdateStatus
        {
            get
            { return _UpdateStatus; }
            set
            {
                if (_UpdateStatus == value)
                    return;
                _UpdateStatus = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region UpdateStatusText変更通知プロパティ
        private string _UpdateStatusText;

        public string UpdateStatusText
        {
            get
            { return _UpdateStatusText; }
            set
            {
                if (_UpdateStatusText == value)
                    return;
                _UpdateStatusText = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
