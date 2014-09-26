using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public delegate void SetText(string value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public delegate void SetValue(int value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public delegate void SetIsIndeterminate(bool value);

    /// <summary>
    /// 各種プロセス用の既定クラスです。
    /// </summary>
    public class Progress
    {
        public SetText Text { set; get; }
        public SetValue Value { set; get; }
        public SetIsIndeterminate Indeterminate { set; get; }

        public void Clear()
        {
            this.Text = null;
            this.Value = null;
            this.Indeterminate = null;
        }

        /// <summary>
        /// ダイアログにテキストを設定します。
        /// </summary>
        /// <param name="value"></param>
        public void SetText(string value)
        {
            Text(value);
        }

        /// <summary>
        /// ダイアログのプログラスバーの値を設定します。 <para />
        /// 最小値は0、最大値は100です。
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            Value(value);
        }

        /// <summary>
        /// ダイアログのプログレスバーを「不定」状態にするかを設定します。
        /// </summary>
        /// <param name="value"></param>
        public void SetIsIndeterminate(bool value)
        {
            Indeterminate(value);
        }
    }
}
