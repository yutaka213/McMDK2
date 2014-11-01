using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace McMDK2.Core.Data
{
    public class ContextMenuItem
    {
        public string Header { set; get; }

        public ICommand Command { set; get; }
    }
}
