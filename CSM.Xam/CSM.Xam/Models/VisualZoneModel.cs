using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualZoneModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region ZoneName
        private string _ZoneName;
        public string ZoneName
        {
            get { return _ZoneName; }
            set { SetProperty(ref _ZoneName, value); }
        }
        #endregion
    }
}
