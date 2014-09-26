using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin;

namespace Fireworks.Process
{
    public class ForgeSupport : Setup
    {
        public override string Id
        {
            get
            {
                return "MinecraftForge";
            }
        }

        public override void Process(string path, string version)
        {
            this.SetIsIndeterminate(false);
            this.SetValue(50);
        }
    }
}
