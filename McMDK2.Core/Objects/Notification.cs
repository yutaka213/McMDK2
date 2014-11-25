using System.Windows.Media;
using Livet;
using Livet.Commands;

#pragma warning disable 1591

namespace McMDK2.Core.Objects
{
    public delegate void LinkClickedCommand(object sender, object args);

    public class Notification : NotificationObject
    {
        public event LinkClickedCommand OnClicked;

        public Notification()
        {
            this.NotificationBorderBrush = Brushes.Salmon;
        }

        #region NotificationText変更通知プロパティ
        private string _NotificationText;

        public string NotificationText
        {
            get
            { return _NotificationText; }
            set
            {
                if (_NotificationText == value)
                    return;
                _NotificationText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region NotificationLikedText変更通知プロパティ
        private string _NotificationLikedText;

        public string NotificationLikedText
        {
            get
            { return _NotificationLikedText; }
            set
            {
                if (_NotificationLikedText == value)
                    return;
                _NotificationLikedText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region NotificationBorderBrush変更通知プロパティ
        private Brush _NotificationBorderBrush;

        public Brush NotificationBorderBrush
        {
            get
            { return _NotificationBorderBrush; }
            set
            {
                if (_NotificationBorderBrush == value)
                    return;
                _NotificationBorderBrush = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region LinkClickedCommand
        private ViewModelCommand _LinkClickedCommand;

        public ViewModelCommand LinkClickedCommand
        {
            get
            {
                if (_LinkClickedCommand == null)
                {
                    _LinkClickedCommand = new ViewModelCommand(LinkClicked);
                }
                return _LinkClickedCommand;
            }
        }

        public void LinkClicked()
        {
            LinkClickedCommand commands = OnClicked;
            if (commands != null)
            {
                OnClicked(this, null);
            }
        }
        #endregion


    }
}
