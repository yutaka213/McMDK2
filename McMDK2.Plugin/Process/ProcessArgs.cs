using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin.Process.Internal;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process
{
    public class ProcessArgs
    {
        /// <summary>
        /// Progress Dialogを操作するメソッドを提供します。
        /// </summary>
        public ProgressSupporter ProgressWindow { private set; get; }

        /// <summary>
        /// イベントのキャンセルをおこないます。<para />
        /// trueを指定することで、該当のイベント以降の動作が行われずに、キャンセルされます。
        /// </summary>
        public bool Cancel { set; get; }


        public ProcessArgs(ProgressSupporter supporter)
        {
            this.ProgressWindow = supporter;
            this.Cancel = false;
        }
    }
}
