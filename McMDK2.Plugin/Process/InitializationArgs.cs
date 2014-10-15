using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process
{
    public class InitializationArgs : ProcessArgs
    {
        /// <summary>
        /// プロジェクトのルートディレクトリ
        /// </summary>
        public string ProjectPath { private set; get; }

        /// <summary>
        /// プロジェクトに含まれるアイテムのリストです。
        /// </summary>
        public ReadOnlyCollection<string> Items { private set; get; }

        public InitializationArgs(string p1, ReadOnlyCollection<string> p2)
        {
            this.ProjectPath = p1;
            this.Items = p2;
        }
    }
}
