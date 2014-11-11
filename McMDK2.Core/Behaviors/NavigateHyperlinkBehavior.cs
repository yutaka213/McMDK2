using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace McMDK2.Core.Behaviors
{
    // https://github.com/karno/StarryEyes/blob/master/StarryEyes/Views/Behaviors/NavigateHyperlinkBehavior.cs
    public class NavigateHyperlinkBehavior : Behavior<Hyperlink>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.RequestNavigate += OnRequestNavigate;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.RequestNavigate -= OnRequestNavigate;
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Define.GetSettings().BrowserFilePath))
                Process.Start(e.Uri.ToString());
            else
                Process.Start(Define.GetSettings().BrowserFilePath, e.Uri.ToString());
        }
    }
}
