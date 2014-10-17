using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.Messaging;
using McMDK2.Core.Converter;

namespace McMDK2.ViewModels.Internal
{
    public class IndirectlyMessenger
    {
        private InteractionMessenger Messenger { set; get; }

        public IndirectlyMessenger(InteractionMessenger messenger)
        {
            this.Messenger = messenger;
        }

        public void Raise(Type windowType, object viewModel, string transitionMode)
        {
            if (viewModel is ViewModel)
            {
                this.Messenger.Raise(new TransitionMessage(windowType, (ViewModel)viewModel, GetTransitionModeFromString(transitionMode), "Transition"));
            }
            else
            {
                this.Messenger.Raise(new TransitionMessage(windowType, null, GetTransitionModeFromString(transitionMode)));
            }
        }

        public void Raise(Type windowType, object viewModel, string transitionMode, string messageKey)
        {
            if (viewModel is ViewModel)
            {
                this.Messenger.Raise(new TransitionMessage(windowType, (ViewModel)viewModel, GetTransitionModeFromString(transitionMode), messageKey));
            }
            else
            {
                this.Messenger.Raise(new TransitionMessage(windowType, null, GetTransitionModeFromString(transitionMode), messageKey));
            }
        }

        public async Task RaiseAsync(Type windowType, object viewModel, string transitionMode)
        {
            if (viewModel is ViewModel)
            {
                await this.Messenger.RaiseAsync(new TransitionMessage(windowType, (ViewModel)viewModel, GetTransitionModeFromString(transitionMode), "Transition"));
            }
            else
            {
                await this.Messenger.RaiseAsync(new TransitionMessage(windowType, null, GetTransitionModeFromString(transitionMode)));
            }
        }

        public async Task RaiseAsync(Type windowType, object viewModel, string transitionMode, string messageKey)
        {
            if (viewModel is ViewModel)
            {
                await this.Messenger.RaiseAsync(new TransitionMessage(windowType, (ViewModel)viewModel, GetTransitionModeFromString(transitionMode)));
            }
            else
            {
                await this.Messenger.RaiseAsync(new TransitionMessage(windowType, null, GetTransitionModeFromString(transitionMode)));
            }
        }

        private TransitionMode GetTransitionModeFromString(string transitionMode)
        {
            return (TransitionMode)StringToObjectConverter.StringToEnum(transitionMode, typeof(TransitionMode), TransitionMode.Normal);
        }
    }
}
