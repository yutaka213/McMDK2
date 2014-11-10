using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using McMDK2.Core;
using McMDK2.Models;
using McMDK2.Plugin;

namespace McMDK2.ViewModels.SettingPages
{
    public class GeneralPageViewModel : ViewModel, IConfiguration
    {
        public void Load()
        {
            this.FeedsCount = Define.GetSettings().ShowBlogPostsCount.ToString();
            this.RecentProjectsCount = Define.GetSettings().RecentProjectsCount.ToString();
            this.IsUseInternetCache = Define.GetSettings().IsUseInternetCache;
            this.IsUseFileCache = Define.GetSettings().IsUseFileCache;
            this.EnableAutoUpdate = Define.GetSettings().AutoUpdate;
        }

        public void Apply()
        {
            Define.GetSettings().ShowBlogPostsCount = int.Parse(this.FeedsCount);
            Define.GetSettings().RecentProjectsCount = int.Parse(this.RecentProjectsCount);
            Define.GetSettings().IsUseInternetCache = this.IsUseInternetCache;
            Define.GetSettings().IsUseFileCache = this.IsUseFileCache;
            Define.GetSettings().AutoUpdate = this.EnableAutoUpdate;
        }


        #region FeedsCount変更通知プロパティ
        private string _FeedsCount;

        public string FeedsCount
        {
            get
            { return _FeedsCount; }
            set
            {
                if (_FeedsCount == value)
                    return;
                _FeedsCount = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region RecentProjectsCount変更通知プロパティ
        private string _RecentProjectsCount;

        public string RecentProjectsCount
        {
            get
            { return _RecentProjectsCount; }
            set
            {
                if (_RecentProjectsCount == value)
                    return;
                _RecentProjectsCount = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsUseInternetCache変更通知プロパティ
        private bool _IsUseInternetCache;

        public bool IsUseInternetCache
        {
            get
            { return _IsUseInternetCache; }
            set
            {
                if (_IsUseInternetCache == value)
                    return;
                _IsUseInternetCache = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsUseFileCache変更通知プロパティ
        private bool _IsUseFileCache;

        public bool IsUseFileCache
        {
            get
            { return _IsUseFileCache; }
            set
            {
                if (_IsUseFileCache == value)
                    return;
                _IsUseFileCache = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region EnableAutoUpdate変更通知プロパティ
        private bool _EnableAutoUpdate;

        public bool EnableAutoUpdate
        {
            get
            { return _EnableAutoUpdate; }
            set
            {
                if (_EnableAutoUpdate == value)
                    return;
                _EnableAutoUpdate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
