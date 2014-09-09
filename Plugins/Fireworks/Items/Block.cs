using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace Fireworks.Items
{
    public class Block : IMod
    {
        public string Name { get { return "ブロック(無機能)"; } }

        public string Version { get { return "1.0.0.0"; } }

        public string Id { get { return "1F121275-6E85-4EED-9A0D-C58B3AC49DA8"; } }

        public ModView View { get { return null; } }
    }
}
