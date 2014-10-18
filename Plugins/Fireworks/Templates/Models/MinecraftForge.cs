using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fireworks.Templates.Models
{
    public class MinecraftForge
    {
        public string Version { set; get; }

        public string MinecraftVersion { set; get; }

        public string ReleaseTime { set; get; }

        public string ChangeLogUri { set; get; }

        public string InstallerUri { set; get; }

        public string InstallerForWinUri { set; get; }

        public string JavadocUri { set; get; }

        public string SrcUri { set; get; }

        public string UniversalUri { set; get; }

        public string UserDevUri { set; get; }

        public string ClientUri { set; get; }

        public string ServerUri { set; get; }

        public override string ToString()
        {
            return this.Version;
        }
    }
}
