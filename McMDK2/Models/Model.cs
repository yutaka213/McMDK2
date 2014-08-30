using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;



namespace McMDK2.Models
{
    public class Model : NotificationObject
    {

        #region CurrentProject変更通知プロパティ
        private object _CurrentProject;

        public object CurrentProject
        {
            get
            { return _CurrentProject; }
            set
            {
                if (_CurrentProject == value)
                    return;
                _CurrentProject = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
