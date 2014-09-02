using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Models;

namespace McMDK2.ViewModels.TabPages
{
    public class StartPageViewModel : ViewModel
    {
        public StartPageViewModel()
        {
            this.BlogFeeds = new ObservableCollection<NewsFeeds>();
            this.RecentProjects = new ObservableCollection<Project>();

#if DEBUG
            var p = new Project();
            p.Name = "Sample Project";

            this.RecentProjects.Add(p);
#endif

            this.IsLoading = true;
            this.UpdateNewsFeeds();
        }

        private string DateToString(string date)
        {
            return DateTime.ParseExact(date, "ddd, d MMM yyyy HH':'mm':'ss zzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None).ToLongDateString();
        }

        private async void UpdateNewsFeeds()
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string r = await client.DownloadStringTaskAsync(new Uri(Define.NewsFeedUrl));

            var q = from p in XDocument.Parse(r).Root.Element("channel").Descendants("item")
                    select new NewsFeeds
                    {
                        Title = p.Element("title").Value,
                        Link = p.Element("link").Value.Replace(Environment.NewLine, ""),
                        PublishDate = DateToString(p.Element("pubDate").Value),
                        Description = p.Element("description").Value.Replace(" &#160; ", "").Replace(" [&#8230;]", "...")
                    };
            int i = 0;
            this.IsLoading = false;
            foreach (var item in q)
            {
                DispatcherHelper.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.BlogFeeds.Add(item);
                    }));
                if (++i >= 5)
                {
                    break;
                }
            }
        }


        #region RecentProjects変更通知プロパティ
        private ObservableCollection<Project> _RecentProjects;

        public ObservableCollection<Project> RecentProjects
        {
            get
            { return _RecentProjects; }
            set
            {
                if (_RecentProjects == value)
                    return;
                _RecentProjects = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region BlogFeeds変更通知プロパティ
        private ObservableCollection<NewsFeeds> _BlogFeeds;

        public ObservableCollection<NewsFeeds> BlogFeeds
        {
            get
            { return _BlogFeeds; }
            set
            {
                if (_BlogFeeds == value)
                    return;
                _BlogFeeds = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsLoading変更通知プロパティ
        private bool _IsLoading;

        public bool IsLoading
        {
            get
            { return _IsLoading; }
            set
            {
                if (_IsLoading == value)
                    return;
                _IsLoading = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OpenUrl

        public void OpenUrl(string parameter)
        {
            if (!String.IsNullOrWhiteSpace(parameter) && (parameter.StartsWith("http://") || parameter.StartsWith("https://")))
            {
                System.Diagnostics.Process.Start(parameter);
            }
        }
        #endregion


        #region UpdateCommand
        private ViewModelCommand _UpdateCommand;

        public ViewModelCommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                {
                    _UpdateCommand = new ViewModelCommand(Update);
                }
                return _UpdateCommand;
            }
        }

        public void Update()
        {

        }
        #endregion


    }
}
