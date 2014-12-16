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
        /// 4桁のバージョンNoを取得します。   <para />
        /// 1.2.5の場合は1250が返ります。
        /// </summary>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static int GetVersionNo(string p1)
        {
            int count = p1.Length - p1.Replace(".", "").Length;
            if (count == 1)
            {
                p1 += ".0";
            }
            if (p1.Length == 5)
            {
                p1 += "0";
            }
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
