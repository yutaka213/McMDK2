using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin;

namespace Fireworks.Process
{
    public class ModLoaderSupport : ISetup
    {
        public string Id
        {
            get { return "ModLoader"; }
        }

        public void Setup(string path, string version)
        {

        }
    }
}
