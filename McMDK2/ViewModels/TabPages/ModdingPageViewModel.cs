using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Plugin;
using McMDK2.Models;
using McMDK2.Plugin;

namespace McMDK2.ViewModels.TabPages
{
    public class ModdingPageViewModel : ViewModel, ItemViewEx
    {
        public ModdingPageViewModel()
        {

        }

        public ModdingPageViewModel(ModView view)
        {
            this.ModdingContent = view;
            this.Loaded = true;
        }

        // Load from *.mod file.
        public void Initialize(string path)
        {
            this.Loaded = false;

            if (FileController.Exists(path))
            {
                try
                {
                    //Deserialize
                    var serializer = new DataContractSerializer(typeof(ItemData));
                    var reader = XmlReader.Create(path);
                    var obj = (ItemData)serializer.ReadObject(reader);

                    var content = ModManager.GetModFromId(obj.PluginId);
                    if (content == null)
                    {
                        this.ErrorText = "アイテムを読込中にエラーが発生しました。";
                        return;
                    }
                    this.ModdingContent = content.View;
                    this.Loaded = true;
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

    }
}
