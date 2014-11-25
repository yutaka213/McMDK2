using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Core.Migrations
{
    public interface IMigratable
    {
        string Version { get; }

        void Migrate();
    }
}
