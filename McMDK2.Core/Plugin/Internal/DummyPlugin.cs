using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace McMDK2.Core.Plugin.Internal
{
    public class DummyPlugin : IPlugin
    {
        public string Name
        {
            get { return "Dummy Plugin"; }
        }

        public string Version
        {
            get { return Define.Version; }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return Guid.Empty.ToString(); }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return null; }
        }

        public string Description
        {
            get { return "Dummy Plugin for McMDK."; }
        }

        public void Loaded() { }

        public void Updated() { }
    }
}
