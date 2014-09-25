using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Objects
{
    public interface Composition
    {
        string DisplayName { set; get; }

        string Id { set; get; }

        void Build();
    }
}
