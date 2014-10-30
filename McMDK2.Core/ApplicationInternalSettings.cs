using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core.Data;

namespace McMDK2.Core
{
    public class ApplicationInternalSettings : ApplicationSettingsBase
    {
        public ApplicationInternalSettings()
            : base("McMDK.Internal.Settings")
        {

        }

        [UserScopedSetting]
        public Project[] RecentProjects
        {
            get
            {
                return (Project[])this["recentprojects"];
            }
            set
            {
                this["recentprojects"] = value;
            }
        }

        [UserScopedSetting]
        public bool IsCheckedAccount
        {
            get
            {
                return (bool)this["ischeckedaccount"];
            }
            set
            {
                this["ischeckedaccount"] = value;
            }
        }

        [UserScopedSetting]
        public string LastOpenedDirectory
        {
            get
            {
                return (string)this["lastopeneddirectory"];
            }
            set
            {
                this["lastopeneddirectory"] = value;
            }
        }
    }
}
