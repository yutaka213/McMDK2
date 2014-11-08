using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Extensions
{
    public static class ListExt
    {
        public static List<T> Clone<T>(this List<T> list) where T : ICloneable
        {
            return new List<T>(list.Select(w => (T)w.Clone()));
        }
    }
}
