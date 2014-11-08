using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Xml;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Models;
using McMDK2.Plugin;

namespace McMDK2.ViewModels.TabPages
{
    public class ProjectSettingPageViewModel : ViewModel
    {
        private readonly MainWindowViewModel MainWindowViewModel;

        public ProjectSettingPageViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.Mods = new ObservableCollection<ItemData>();

            var items = this.MainWindowViewModel.SearchItem(w => w.FileType == Define.IdentifierMods);
            foreach (var item in items)
            {
                var serializer = new DataContractSerializer(typeof(ItemData));
                using (var reader = XmlReader.Create(item.FilePath))
                {
                    this.Mods.Add((ItemData)serializer.ReadObject(reader));
                }
            }

        }


        #region McModInfo変更通知プロパティ
        private ModInfo _McModInfo;

        public ModInfo McModInfo
        {
            get
            { return _McModInfo; }
            set
            {
                if (_McModInfo == value)
                    return;
                _McModInfo = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Mods変更通知プロパティ
        private ObservableCollection<ItemData> _Mods;

        public ObservableCollection<ItemData> Mods
        {
            get
            { return _Mods; }
            set
            {
                if (_Mods == value)
                    return;
                _Mods = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
