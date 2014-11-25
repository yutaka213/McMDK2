using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#pragma warning disable 1591

namespace McMDK2.Core.Data
{
    /// <summary>
    /// ContextMenu
    /// </summary>
    public class ContextMenuItem
    {
        public string Header { set; get; }

        public ICommand Command { set; get; }
    }
}
