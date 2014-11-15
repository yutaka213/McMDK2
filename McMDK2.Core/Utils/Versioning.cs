using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Utils
{
    public static class Versioning
    {
        public static int GetVersionNo(string p1)
        {
            return int.Parse(p1.Replace(".", ""));
        }

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
