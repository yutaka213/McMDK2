using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Behaviors.Internal
{
    /// <summary>
    /// 遅いけども、全てのロード済みアセンブリから動的にイベントを呼び出すのには
    /// コレしか思いつかなかった。
    /// </summary>
    internal static class AsmResolver
    {
        private static readonly Dictionary<string, Type> Cache = new Dictionary<string, Type>();

        public static Type GetTypeFromString(string name)
        {
            if (Cache.ContainsKey(name))
                return Cache[name];

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms)
            {
                Type t = asm.GetType(name);
                if (t != null)
                {
                    Cache.Add(name, t);
                    return t;
                }
            }
            return null;
        }
    }
}
