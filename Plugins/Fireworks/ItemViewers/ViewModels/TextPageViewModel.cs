using System;
using System.Collections.Generic;
using System.IO;
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
using McMDK2.Plugin;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Fireworks.ItemViewers.ViewModels
{
    public class TextPageViewModel : ViewModel, ItemViewEx
    {
        private string LoadedText;
        private string Path;

        public void Initialize(string path)
        {
            this.LoadedText = "";
            this.Path = "";
            this.Loaded = false;

            if (FileController.Exists(path))
            {
                try
                {
                    using (var sr = new StreamReader(path))
                    {
                        this.Text = sr.ReadToEnd();
                        this.LoadedText = this.Text;
                        this.Path = path;
                        this.Loaded = true;
                    }
                }
                catch (Exception e)
                {
                    this.ErrorText = e.Message;
                }
                return;
            }
            this.ErrorText = "ファイルが存在しないため、編集できません。";
        }

        public void Closing()
        {
            if (this.Text != this.LoadedText)
            {
                var taskDialog = new TaskDialog
                {
                    Caption = "Alert",
                    InstructionText = String.Format("ファイル {0} は変更されています。", System.IO.Path.GetFileName(this.Path)),
                    Text = String.Format("ファイル {0} は変更されています。" + Environment.NewLine + "変更を保存してからタブを閉じますか？", System.IO.Path.GetFileName(this.Path)),
                    Icon = TaskDialogStandardIcon.Error,
                    StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No
                };
                taskDialog.Opened += (_sender, _e) =>
                {
                    ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                };
                if (taskDialog.Show() == TaskDialogResult.Yes)
                {
                    // Edited
                    using (var sw = new StreamWriter(this.Path))
                    {
                        sw.WriteLine(this.Text);
                    }
                }
            }
        }


        #region ErrorText変更通知プロパティ
        private string _ErrorText;

        public string ErrorText
        {
            get
            { return _ErrorText; }
            set
            {
                if (_ErrorText == value)
                    return;
                _ErrorText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Loaded変更通知プロパティ
        private bool _Loaded;

        public bool Loaded
        {
            get
            { return _Loaded; }
            set
            {
                if (_Loaded == value)
                    return;
                _Loaded = value;
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
