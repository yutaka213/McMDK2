using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Net;
using McMDK2.Core.Utils;
using McMDK2.Models;
using McMDK2.Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                if (FileController.Exists(item.FilePath))
                {
                    var serializer = new DataContractSerializer(typeof(ItemData));
                    using (var reader = XmlReader.Create(item.FilePath))
                    {
                        this.Mods.Add((ItemData)serializer.ReadObject(reader));
                    }
                }
            }
            object modinfo = this.MainWindowViewModel.CurrentProject.ProjectSettings["modinfo"];
            if (modinfo is ModInfo)
                this.McModInfo = (ModInfo)modinfo;
            else
                this.McModInfo = JsonConvert.DeserializeObject<ModInfo>(modinfo.ToString());
            this.ProjectName = this.MainWindowViewModel.CurrentProject.Name;

            this.McVersion = (string)this.MainWindowViewModel.CurrentProject.ProjectSettings["mcversion"];
            this.McVersions = new ObservableCollection<string>();
            Task.Run(() =>
            {
                if (Define.IsOfflineMode)
                {
                    string mcdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
                    string versionsDir = Path.Combine(mcdir, "versions", "versions.json");
                    var json = JObject.Parse((new StreamReader(versionsDir)).ReadToEnd());
                    foreach (var jobj in (JArray)json["versions"])
                    {
                        if ((string)jobj["type"] == "release")
                        {
                            if (Versioning.GetVersionNo((string)jobj["id"]) >= 125)
                            {
                                DispatcherHelper.UIDispatcher.Invoke(() =>
                                {
                                    this.McVersions.Add((string)jobj["id"]);
                                });
                            }
                        }
                    }
                }
                else
                {
                    var json = JArray.Parse(SimpleHttp.Get(Define.ApiVersionsList));
                    foreach (var jobj in json)
                    {
                        DispatcherHelper.UIDispatcher.Invoke(() =>
                        {
                            this.McVersions.Add((string)jobj["Version"]);
                        });
                    }
                }
            });
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


        #region ProjectName変更通知プロパティ
        private string _ProjectName;

        public string ProjectName
        {
            get
            { return _ProjectName; }
            set
            {
                if (_ProjectName == value)
                    return;
                _ProjectName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region McVersions変更通知プロパティ
        private ObservableCollection<string> _McVersions;

        public ObservableCollection<string> McVersions
        {
            get
            { return _McVersions; }
            set
            {
                if (_McVersions == value)
                    return;
                _McVersions = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region McVersion変更通知プロパティ
        private string _McVersion;

        public string McVersion
        {
            get
            { return _McVersion; }
            set
            {
                if (_McVersion == value)
                    return;
                _McVersion = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OutputName変更通知プロパティ
        private string _OutputName;

        public string OutputName
        {
            get
            { return _OutputName; }
            set
            {
                if (_OutputName == value)
                    return;
                _OutputName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OutputLoc変更通知プロパティ
        private string _OutputLoc;

        public string OutputLoc
        {
            get
            { return _OutputLoc; }
            set
            {
                if (_OutputLoc == value)
                    return;
                _OutputLoc = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
