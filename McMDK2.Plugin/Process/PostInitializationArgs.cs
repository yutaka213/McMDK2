using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin.Process.Internal;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process
{
    public class PostInitializationArgs : ProcessArgs
    {
        /// <summary>
        /// プロジェクトのルートディレクトリ
        /// </summary>
        public string ProjectPath { private set; get; }

        /// <summary>
        /// Window遷移用のサポートクラス
        /// </summary>
        public WindowTransitionSupporter WindowTransition { private set; get; }

        public PostInitializationArgs(string p1, WindowTransitionSupporter p2, ProgressSupporter p3)
            : base(p3)
        {
            this.ProjectPath = p1;
            this.WindowTransition = p2;
        }
    }
}
