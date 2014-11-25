using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core
{
    /// <summary>
    /// 値のチェック機能を提供します。
    /// </summary>
    public static class ValueChecker
    {
        /// <summary>
        /// 配列が全てnullの場合にTrueを返します。
        /// </summary>
        public static bool IsNull(params object[] objects)
        {
            return objects.All(obj => obj == null);
        }

        /// <summary>
        /// 配列が全て!nullの場合にTrueを返します。
        /// </summary>
        public static bool IsNotNull(params object[] objects)
        {
            return objects.All(obj => obj != null);
        }

        /// <summary>
        /// objectがint型の数値の場合にtrueを返します。
        /// </summary>
        public static bool IsInt32(object value)
        {
            int i;
            if (int.TryParse(value.ToString(), out i))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// objectがbool型の文字列または数値の場合にtrueを返します。
        /// </summary>
        public static bool IsBool(object value)
        {
            bool b;
            if (bool.TryParse(value.ToString(), out b))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// objectがshort型の数値の場合にtrueを返します。
        /// </summary>
        public static bool IsShort(object value)
        {
            short s;
            if (short.TryParse(value.ToString(), out s))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// objectがbyte型の数値の場合にtrueを返します。
        /// </summary>
        public static bool IsByte(object value)
        {
            byte b;
            if (byte.TryParse(value.ToString(), out b))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// objectがlong型の数値の場合にtrueを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLong(object value)
        {
            long l;
            if (long.TryParse(value.ToString(), out l))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// objectがdouble型の数値の場合にtrueを返します。<para/>
        /// ただし、指数表記のEが含まれている場合はfalseを返します。
        /// </summary>
        public static bool IsDouble(object value)
        {
            double d;
            if (double.TryParse(value.ToString(), out d))
            {
                if (value.ToString().Contains("E") || value.ToString().Contains("e"))
                {
                    //間違ってはないけど、都合がわるいのでさようなら
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// objectがfloat型の数値の場合にtrueを返します。<para/>
        /// ただし指数表記のEが含まれている場合はfalseを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFloat(object value)
        {
            float f;
            if (float.TryParse(value.ToString(), out f))
            {
                if (value.ToString().Contains("E") || value.ToString().Contains("e"))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// valueの値がmin以上、max以下の場合にtrueを返します。
        /// </summary>
        public static bool Between(int value, int max = int.MaxValue, int min = int.MinValue)
        {
            // ReSharper disable ReplaceWithSingleAssignment.True
            bool flag = true;
            if (max <= value)
            {
                flag = false;
            }
            if (min >= value)
            {
                flag = false;
            }
            return flag;
        }
    }
}
