using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Utils
{
    /// <summary>
    /// バージョンに関する機能を提供します。
    /// </summary>
    public static class Versioning
    {
        /// <summary>
        /// バージョンNoを取得します。
        /// </summary>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static int GetVersionNo(string p1)
        {
            return int.Parse(p1.Replace(".", ""));
        }

        /// <summary>
        /// バージョンを比較します。
        /// </summary>
        public static int Compare(string p1, string p2)
        {
            int a1 = int.Parse(p1.Replace(".", ""));
            int a2 = int.Parse(p2.Replace(".", ""));
            if (a1 < a2)
                return -1;
            if (a1 == a2)
                return 0;
            return 1;
        }
    }
}
