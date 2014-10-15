using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process
{
    public class PostInitializationArgs : ProcessArgs
    {
        /// <summary>
        /// プロジェクトのルートディレクトリ
        /// </summary>
        public string ProjectPath { private set; get; }

        public PostInitializationArgs(string p1)
        {
            this.ProjectPath = p1;
        }
    }
}
