using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin;

namespace Fireworks.Process
{
    public class ForgeSupport : ISetup
    {
        public string Id
        {
            get { return "MinecraftForge"; }
        }

        public void Setup(string path, string version)
        {

        }
    }
}
