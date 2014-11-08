using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace McMDK2.Core
{
    public class ApplicationSettings : ApplicationSettingsBase
    {
        public ApplicationSettings()
            : base("McMDK.Settings")
        {

        }

        [UserScopedSetting]
        [DefaultSettingValue("5")]
        public int RecentProjectsCount
        {
            get
            {
                return (int)this["recentprojectscount"];
            }
            set
            {
                this["recentprojectscount"] = value;
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("10")]
        public int ShowBlogPostsCount
        {
            get
            {
                return (int)this["showblogpostscount"];
            }
            set
            {
                this["showblogpostscount"] = value;
            }
        }
    }
}
