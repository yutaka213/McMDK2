using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Extensions
{
    public static class ObservableCollectionExt
    {
        public static ObservableCollection<T> Clone<T>(this ObservableCollection<T> collection) where T : ICloneable
        {
            return new ObservableCollection<T>(collection.Select(w => (T)w.Clone()));
        }
    }
}
