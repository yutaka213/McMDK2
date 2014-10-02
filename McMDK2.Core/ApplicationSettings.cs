using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace McMDK2.Core
{
    public class ApplicationSettings
    {
        public _ApplicationSettings Settings { set; get; }

        public void Reload()
        {
            if (Settings == null)
            {
                Settings = new _ApplicationSettings();
            }
            Settings.Reload();
        }

        public void Save()
        {
            if (Settings == null)
            {
                Settings = new _ApplicationSettings();
            }
            Settings.Save();
        }

        [DataContract]
        public class _ApplicationSettings
        {
            [DataMember]
            public Dictionary<string, object> _values = new Dictionary<string, object>();

            public object this[string key]
            {
                set { this._values[key] = value; }
                get { return this._values[key]; }
            }

            public void Save()
            {
                var serializer = new DataContractSerializer(this.GetType());
                var writer = XmlWriter.Create(Define.SettingFile);
                serializer.WriteObject(writer, this);
                writer.Close();
            }

            public void Reload()
            {
                var serializer = new DataContractSerializer(this.GetType());
                var reader = XmlReader.Create(Define.SettingFile);
                this._values = (Dictionary<string, object>)serializer.ReadObject(reader);
                reader.Close();
            }
        }
    }
}
