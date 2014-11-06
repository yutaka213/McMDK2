using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Plugin.Internal
{
    internal static class IdStore
    {
        private static readonly Dictionary<string, Type> store = new Dictionary<string, Type>();

        public static void RegisterId(string id, Type type)
        {
            if (store.ContainsKey(id))
                return;
            store.Add(id, type);
        }

        public static Type GetTypeFromId(string id)
        {
            return store[id];
        }
    }
}
