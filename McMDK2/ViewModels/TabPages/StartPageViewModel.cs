using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core.Data;
using McMDK2.Models;

namespace McMDK2.ViewModels.TabPages
{
    public class StartPageViewModel : ViewModel
    {
        public StartPageViewModel()
        {
            this.BlogFeeds = new ObservableCollection<NewsFeeds>();

#if DEBUG
            /* TEST DATA */
            NewsFeeds feeds1 = new NewsFeeds();
            feeds1.Title = "【Minecraft】McMDK進捗報告 2014/06/07";
            feeds1.Link = "http://blog.tuyapin.net/2014/06/07/%e3%80%90minecraft%e3%80%91mcmdk%e9%80%b2%e6%8d%97%e5%a0%b1%e5%91%8a-20140607/";
            feeds1.PublishDate = DateToString("Fri, 06 Jun 2014 19:34:52 +0000");
            feeds1.Description = "響がВерныйになりました。ということでいつもどおりの進捗報告です。今回の更新点は テクスチャの動的生成 だけです。クライアントサイドではいまはWebページの方を作成しています。と...";
            this.BlogFeeds.Add(feeds1);

            NewsFeeds feeds2 = new NewsFeeds();
            feeds2.Title = "【Minecraft】McMDK進捗報告 2014/06/02";
            feeds2.Link = "http://blog.tuyapin.net/2014/06/02/%e3%80%90minecraft%e3%80%91mcmdk%e9%80%b2%e6%8d%97%e5%a0%b1%e5%91%8a-20140602/";
            feeds2.PublishDate = DateToString("Sun, 01 Jun 2014 17:54:15 +0000");
            feeds2.Description = "借金だわーいあはははは こんばんは、こんな深夜にもがんばってるつやぴんさんです。 来週テストだから今週は作業あんまりしません、よろしく。あれ、デジャヴを感じます。なんでだろう？ということで...";
            this.BlogFeeds.Add(feeds2);

            NewsFeeds feeds3 = new NewsFeeds();
            feeds3.Title = "【Minecraft】McMDK進捗報告 2014/06/01";
            feeds3.Link = "http://blog.tuyapin.net/2014/06/01/%e3%80%90minecraft%e3%80%91mcmdk%e9%80%b2%e6%8d%97%e5%a0%b1%e5%91%8a-20140601/";
            feeds3.PublishDate = DateToString("Sat, 31 May 2014 18:45:30 +0000");
            feeds3.Description = "かっこわらい こんばんは、こんな深夜にもがんばってるつやぴんさんです。 来週テストだから今週は作業あんまりしません、よろしく。Minecraft Forge #140 ~ #171(1.2.5) Mine...";
            this.BlogFeeds.Add(feeds3);

            NewsFeeds feeds4 = new NewsFeeds();
            feeds4.Title = "【Minecraft】McMDK進捗報告 2014/05/20";
            feeds4.Link = "http://blog.tuyapin.net/2014/05/20/%e3%80%90minecraft%e3%80%91mcmdk%e9%80%b2%e6%8d%97%e5%a0%b1%e5%91%8a-20140520/";
            feeds4.PublishDate = DateToString("Mon, 19 May 2014 17:55:47 +0000");
            feeds4.Description = "まんじゅうこわい おはこんばんちは　つやぴんさんだよ！ということで久しぶりに進捗報告です。 そのまえにちょっとした変更を McMDKのリポジトリが https://github.com/tuyapin/M...";
            this.BlogFeeds.Add(feeds4);
#endif
        }

#if DEBUG
        private string DateToString(string date)
        {
            return DateTime.ParseExact(date, "ddd, d MMM yyyy HH':'mm':'ss zzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None).ToLongDateString();
        }
#endif

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

    }
}
