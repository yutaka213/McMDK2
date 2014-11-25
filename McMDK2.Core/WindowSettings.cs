using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using McMDK2.Core.Win32;

#pragma warning disable 1591

namespace McMDK2.Core
{
    // http://grabacr.net/archives/1585
    public class WindowSettings : ApplicationSettingsBase
    {
        public WindowSettings(Window window)
            : base(window.GetType().FullName)
        {
        }

        [UserScopedSetting]
        public WinApi.WINDOWPLACEMENT? Placement
        {
            get
            {
                return this["Placement"] != null ? (WinApi.WINDOWPLACEMENT?)(WinApi.WINDOWPLACEMENT)this["Placement"] : null;
            }
            set
            {
                this["Placement"] = value;
            }
        }
    }
}
