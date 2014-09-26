using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin;

namespace Fireworks.Process
{
    public class ModLoaderSupport : Setup
    {
        public override string Id
        {
            get { return "ModLoader"; }
        }

        public override void Process(string path, string version)
        {
            throw new NotImplementedException();
        }
    }
}
