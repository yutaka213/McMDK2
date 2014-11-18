using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process.Internal
{
    /// <summary>
    /// Progress Dialog ViewModel を直接触らずに、Progress Dialogを操作する基本的な機能を提供します。
    /// </summary>
    public class ProgressSupporter
    {
        private Delegate_V_S1 Delegate_1 { set; get; }
        private Delegate_V_I1 Delegate_2 { set; get; }
        private Delegate_V_B1 Delegate_3 { set; get; }
        private Delegate_V_S1 Delegate_4 { set; get; }

        public ProgressSupporter(Delegate_V_S1 p1, Delegate_V_I1 p2, Delegate_V_B1 p3, Delegate_V_S1 p4)
        {
            this.Delegate_1 = p1;
            this.Delegate_2 = p2;
            this.Delegate_3 = p3;
            this.Delegate_4 = p4;
        }

        public void SetText(string text)
        {
            this.Delegate_1(text);
        }

        public void SetTaskText(string text)
        {
            this.Delegate_4(text);
        }

        public void SetProgressValue(int value)
        {
            this.Delegate_2(value);
        }

        public void SetIsIndetermiate(bool value)
        {
            this.Delegate_3(value);
        }

        public delegate void Delegate_V_S1(string p1);

        public delegate void Delegate_V_I1(int p1);

        public delegate void Delegate_V_B1(bool p1);
    }

}
