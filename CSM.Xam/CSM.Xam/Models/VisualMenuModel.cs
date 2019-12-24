using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualMenuModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region MenuName
        private string _MenuName;
        public string MenuName
        {
            get { return _MenuName; }
            set { SetProperty(ref _MenuName, value); }
        }
        #endregion

        #region ImageIcon
        private string _ImageIcon;
        public string ImageIcon
        {
            get { return _ImageIcon; }
            set { SetProperty(ref _ImageIcon, value); }
        }
        #endregion

        #region IsDeleted
        private long _IsDeleted;
        public long IsDeleted
        {
            get { return _IsDeleted; }
            set { SetProperty(ref _IsDeleted, value); }
        }
        #endregion
    }
}
