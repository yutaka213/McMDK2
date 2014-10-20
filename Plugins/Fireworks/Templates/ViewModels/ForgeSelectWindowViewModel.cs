using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using Fireworks.Templates.Models;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core;
using McMDK2.Core.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fireworks.Templates.ViewModels
{
    public class ForgeSelectWindowViewModel : ViewModel
    {
        private Dictionary<string, object> obj;

        public ForgeSelectWindowViewModel(string mc, Dictionary<string, object> obj)
        {
            this.McVersion = mc;
            this.obj = obj;
            this.Versions = new ObservableCollection<MinecraftForge>();
            this.SelectedVersion = null;
        }

        public void Initialize()
        {
            Task.Run(() =>
            {
                var json = JArray.Parse(SimpleHttp.Get(String.Format("https://api.tuyapin.net/mcmdk/2/forge/version/{0}.json", this.McVersion)));
                foreach (var jobj in json)
                {
                    DispatcherHelper.UIDispatcher.Invoke(() =>
                    {
                        this.Versions.Add(JsonConvert.DeserializeObject<MinecraftForge>(jobj.ToString()));
                    });
                }
            });
        }


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


        #region Versions変更通知プロパティ
        private ObservableCollection<MinecraftForge> _Versions;

        public ObservableCollection<MinecraftForge> Versions
        {
            get
            { return _Versions; }
            set
            {
                if (_Versions == value)
                    return;
                _Versions = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedVersion変更通知プロパティ
        private MinecraftForge _SelectedVersion;

        public MinecraftForge SelectedVersion
        {
            get
            { return _SelectedVersion; }
            set
            {
                if (_SelectedVersion == value)
                    return;
                _SelectedVersion = value;
                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region OKCommand
        private ViewModelCommand _OKCommand;

        public ViewModelCommand OKCommand
        {
            get
            {
                if (_OKCommand == null)
                {
                    _OKCommand = new ViewModelCommand(OK, CanOK);
                }
                return _OKCommand;
            }
        }

        public void OK()
        {
            this.obj.Add("Source", this.SelectedVersion.SrcUri);
            this.obj.Add("MinecraftForge", this.SelectedVersion.Version);
            this.obj.Add("Build", "MinecraftForge");
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }

        public bool CanOK()
        {
            if (this.SelectedVersion == null)
                return false;
            return true;
        }

        #endregion

    }
}
