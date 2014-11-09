﻿using System;
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

        /// <summary>
        /// 最近開いたプロジェクトの表示数
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("5")]
        public int RecentProjectsCount
        {
            get { return (int)this["recentprojectscount"]; }
            set { this["recentprojectscount"] = value; }
        }

        /// <summary>
        /// スタートページに表示される「ニュースフィード」の表示数
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("10")]
        public int ShowBlogPostsCount
        {
            get { return (int)this["showblogpostscount"]; }
            set { this["showblogpostscount"] = value; }
        }

        /// <summary>
        /// HTTP通信時にキャッシュを利用するか
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool IsUseInternetCache
        {
            get { return (bool)this["isuseinternetcache"]; }
            set { this["isuseinternetcache"] = value; }
        }

        /// <summary>
        /// GradleやForgeなどでキャッシュを利用するか
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool IsUseFileCache
        {
            get { return (bool)this["isusefilecache"]; }
            set { this["isusefilecache"] = value; }
        }

        /// <summary>
        /// 自動アップデートを行うかどうか
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool AutoUpdate
        {
            get { return (bool)this["autoupdate"]; }
            set { this["autoupdate"] = value; }
        }
    }
}
