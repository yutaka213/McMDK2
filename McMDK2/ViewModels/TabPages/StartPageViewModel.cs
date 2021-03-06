﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Linq;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Objects;
using McMDK2.Models;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK2.ViewModels.TabPages
{
    public class StartPageViewModel : ViewModel
    {
        private readonly MainWindowViewModel MainWindowViewModel;

        public StartPageViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.BlogFeeds = new ObservableCollection<NewsFeeds>();
            this.RecentProjects = new ObservableCollection<Project>();
            this.Notifications = new ObservableCollection<Notification>();
            this.StatusMessage = "取得中...";
            this.Version = Define.Version;
            this.IsLoading = true;

            this.RecentProjects = this.MainWindowViewModel.RecentProjects;

            if (Define.FoundNewVersion)
            {
                var item = new Notification();
                item.NotificationText = "最新版が公開されています。" + Environment.NewLine + "「更新する」をクリックして、更新を行ってください。";
                item.NotificationLikedText = "更新する";
                item.NotificationBorderBrush = Brushes.Red;

                this.Notifications.Add(item);
            }

            this.UpdateNewsFeeds();
        }

        private string DateToString(string date)
        {
            return DateTime.ParseExact(date, "ddd, d MMM yyyy HH':'mm':'ss zzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None).ToLongDateString();
        }

        private async void UpdateNewsFeeds()
        {
            if (!Define.IsOfflineMode)
            {
                try
                {
                    var client = new WebClient
                    {
                        Encoding = Encoding.UTF8
                    };
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
                        if (++i >= Define.GetSettings().ShowBlogPostsCount)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                    this.IsLoading = true;
                    this.StatusMessage = "ニュースフィードを読み込むことができませんでした。";
                    Define.GetLogger().Error("Cannot connect to blog RSS feeds.");
                }
            }
            else
            {
                this.IsLoading = true;
                this.StatusMessage = "オフラインモードのため、フィードを読み込みませんでした。";
            }
        }

        #region Version変更通知プロパティ
        private string _Version;

        public string Version
        {
            get
            { return _Version; }
            set
            {
                if (_Version == value)
                    return;
                _Version = value;
                RaisePropertyChanged();
            }
        }
        #endregion


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


        #region Notifications変更通知プロパティ
        private ObservableCollection<Notification> _Notifications;

        public ObservableCollection<Notification> Notifications
        {
            get
            { return _Notifications; }
            set
            {
                if (_Notifications == value)
                    return;
                _Notifications = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region StatusMessage変更通知プロパティ
        private string _StatusMessage;

        public string StatusMessage
        {
            get
            { return _StatusMessage; }
            set
            {
                if (_StatusMessage == value)
                    return;
                _StatusMessage = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OpenUrl

        public void OpenUrl(string parameter)
        {
            if (!String.IsNullOrWhiteSpace(parameter) && (parameter.StartsWith("http://") || parameter.StartsWith("https://")))
            {
                if (String.IsNullOrWhiteSpace(Define.GetSettings().BrowserFilePath))
                    Process.Start(parameter);
                else
                    Process.Start(Define.GetSettings().BrowserFilePath, parameter);
            }
        }
        #endregion


        #region OpenRecentProject

        public void OpenRecentProject(Project parameter)
        {
            if (FileController.Exists(Path.Combine(parameter.Path, parameter.Name + ".mmproj")))
            {
                if (this.MainWindowViewModel.CurrentProject != null)
                    this.MainWindowViewModel.CurrentProject.Save();

                this.MainWindowViewModel.OpenProject(Path.Combine(parameter.Path, "project.mdk"));
                this.MainWindowViewModel.Tabs.Clear();

            }
            else
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "Error";
                taskDialog.InstructionText = "プロジェクトファイルを読み込めませんでした。";
                taskDialog.Text = "プロジェクトファイル(*.mmproj)を見つけることができなかったか、ファイルフォーマットが正常でない可能性があります。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (_sender, _e) =>
                {
                    ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                };
                taskDialog.Show();
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


        #region NewProjectCommand
        private ViewModelCommand _NewProjectCommand;

        public ViewModelCommand NewProjectCommand
        {
            get
            {
                if (_NewProjectCommand == null)
                {
                    _NewProjectCommand = new ViewModelCommand(NewProject);
                }
                return _NewProjectCommand;
            }
        }

        public void NewProject()
        {
            this.MainWindowViewModel.NewProject();
        }
        #endregion


        #region OpenProjectCommand
        private ViewModelCommand _OpenProjectCommand;

        public ViewModelCommand OpenProjectCommand
        {
            get
            {
                if (_OpenProjectCommand == null)
                {
                    _OpenProjectCommand = new ViewModelCommand(OpenProject);
                }
                return _OpenProjectCommand;
            }
        }

        public void OpenProject()
        {
            this.MainWindowViewModel.OpenProject();
        }
        #endregion

    }
}
