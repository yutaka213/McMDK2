using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace McMDK2.Core.Plugin.Internal
{
    public class XmlBaseMod : IMod
    {
        public string Name { set; get; }

        public string Version { set; get; }

        public string Id { set; get; }

        public ModView View { set; get; }
    }
}
