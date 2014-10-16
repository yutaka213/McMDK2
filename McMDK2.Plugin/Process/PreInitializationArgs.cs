using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin.Process.Internal;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process
{

    public class PreInitializationArgs : ProcessArgs
    {
        /// <summary>
        /// プロジェクトのルートディレクトリ
        /// </summary>
        public string ProjectPath { private set; get; }

        /// <summary>
        /// Window遷移用のサポートクラス
        /// </summary>
        public WindowTransitionSupporter WindowTransition { private set; get; }

        /// <summary>
        /// project.mdkのユーザー拡張プロパティ
        /// </summary>
        public Dictionary<string, object> UserProperties { private set; get; }

        public PreInitializationArgs(string p1, Dictionary<string, object> p2, WindowTransitionSupporter p3, ProgressSupporter p4)
            : base(p4)
        {
            this.ProjectPath = p1;
            this.UserProperties = p2;
            this.WindowTransition = p3;
        }
    }
}
