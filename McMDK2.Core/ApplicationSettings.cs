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

        // ======================================================
        // General settings
        // ------------------------------------------------------
        [UserScopedSetting]
        [DefaultSettingValue("10")]
        public int ShowBlogPostsCount
        {
            get { return (int)this["showblogpostscount"]; }
            set { this["showblogpostscount"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("5")]
        public int RecentProjectsCount
        {
            get { return (int)this["recentprojectscount"]; }
            set { this["recentprojectscount"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool IsUseInternetCache
        {
            get { return (bool)this["isuseinternetcache"]; }
            set { this["isuseinternetcache"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool IsUseFileCache
        {
            get { return (bool)this["isusefilecache"]; }
            set { this["isusefilecache"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool AutoUpdate
        {
            get { return (bool)this["autoupdate"]; }
            set { this["autoupdate"] = value; }
        }

        // ======================================================
        // Browser setting
        // ------------------------------------------------------
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string BrowserFilePath
        {
            get { return (string)this["browserfilepath"]; }
            set { this["browserfilepath"] = value; }
        }

        // ======================================================
        // Java settings
        // ------------------------------------------------------
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string JavawFilePath
        {
            get { return (string)this["javawfilepath"]; }
            set { this["javawfilepath"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string JVMArguments
        {
            get { return (string)this["jvmarguments"]; }
            set { this["jvmarguments"] = value; }
        }
    }
}
