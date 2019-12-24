using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class SideBarModel : BindableBase
    {
        #region Title
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        #endregion

        #region Icon
        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set { SetProperty(ref _Icon, value); }
        }
        #endregion
    }
}
