using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualItemModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region ItemName
        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set { SetProperty(ref _ItemName, value); }
        }
        #endregion

        #region MinQuantity
        private long _MinQuantity;
        public long MinQuantity
        {
            get { return _MinQuantity; }
            set { SetProperty(ref _MinQuantity, value); }
        }
        #endregion

        #region CurrentQuantity
        private long _CurrentQuantity;
        public long CurrentQuantity
        {
            get { return _CurrentQuantity; }
            set { SetProperty(ref _CurrentQuantity, value); }
        }
        #endregion
    }
}
