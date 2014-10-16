using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Plugin.Process.Internal
{
    /// <summary>
    /// ViewModelBase.Messenger.Raise/RaiseAsyncを操作する基本的な機能を提供します。
    /// </summary>
    public class WindowTransitionSupporter
    {
        private Delegate_V_TOS Delegate_1 { set; get; }
        private Delegate_V_TOSS Delegate_2 { set; get; }
        private Delegate_T_TOS Delegate_3 { set; get; }
        private Delegate_T_TOSS Delegate_4 { set; get; }

        public WindowTransitionSupporter(Delegate_V_TOS p1, Delegate_V_TOSS p2, Delegate_T_TOS p3, Delegate_T_TOSS p4)
        {
            this.Delegate_1 = p1;
            this.Delegate_2 = p2;
            this.Delegate_3 = p3;
            this.Delegate_4 = p4;
        }

        public void Raise(Type windowType, object viewModel, string transitionMode)
        {
            this.Delegate_1(windowType, viewModel, transitionMode);
        }

        public void Raise(Type windowType, object viewModel, string transitionMode, string messageKey)
        {
            this.Delegate_2(windowType, viewModel, transitionMode, messageKey);
        }

        public async void RaiseAsync(Type windowType, object viewModel, string transitionMode)
        {
            await this.Delegate_3(windowType, viewModel, transitionMode);
        }

        public async void RaiseAsync(Type windowType, object viewModel, string transitionMode, string messageKey)
        {
            await this.Delegate_4(windowType, viewModel, transitionMode, messageKey);
        }

        public delegate void Delegate_V_TOS(Type p1, object p2, string p3);

        public delegate void Delegate_V_TOSS(Type p1, object p2, string p3, string p4);

        public delegate Task Delegate_T_TOS(Type p1, object p2, string p3);

        public delegate Task Delegate_T_TOSS(Type p1, object p2, string p3, string p4);
    }
}
