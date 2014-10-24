using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core;
using McMDK2.Plugin;

namespace Fireworks.ItemViewers.ViewModels
{
    public class ImagePageViewModel : ViewModel, ItemViewEx
    {

        public void Initialize(string path)
        {
            this.MaxHeight = double.MaxValue;
            this.MaxWidth = double.MaxValue;
            this.Loaded = false;

            if (FileController.Exists(path))
            {
                try
                {
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();

                        this.ImagePath = bitmap;
                        this.MaxHeight = bitmap.PixelHeight;
                        this.MaxWidth = bitmap.PixelWidth;
                        this.Loaded = true;

                    }
                }
                catch (Exception e)
                {
                    this.ErrorText = e.Message;
                }
                return;
            }
            this.ErrorText = "この項目では画像プレビューを利用することはできません。";

        }

        public void Closing()
        {
        }


        #region ImagePath変更通知プロパティ
        private BitmapImage _ImagePath;

        public BitmapImage ImagePath
        {
            get
            { return _ImagePath; }
            set
            {
                if (_ImagePath == value)
                    return;
                _ImagePath = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MaxHeight変更通知プロパティ
        private double _MaxHeight;

        public double MaxHeight
        {
            get
            { return _MaxHeight; }
            set
            {
                if (_MaxHeight == value)
                    return;
                _MaxHeight = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MaxWidth変更通知プロパティ
        private double _MaxWidth;

        public double MaxWidth
        {
            get
            { return _MaxWidth; }
            set
            {
                if (_MaxWidth == value)
                    return;
                _MaxWidth = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ErrorText変更通知プロパティ
        private string _ErrorText;

        public string ErrorText
        {
            get
            { return _ErrorText; }
            set
            {
                if (_ErrorText == value)
                    return;
                _ErrorText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Loaded変更通知プロパティ
        private bool _Loaded;

        public bool Loaded
        {
            get
            { return _Loaded; }
            set
            {
                if (_Loaded == value)
                    return;
                _Loaded = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
