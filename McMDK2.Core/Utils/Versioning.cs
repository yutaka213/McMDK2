using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Utils
{
    public class Versioning
    {
        public static int GetVersionNo(string p1)
        {
            return int.Parse(p1.Replace(".", ""));
        }
    }
}
