using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualItemMenuModel : BindableBase
    {
        public string Id { get; set; }

        #region Name
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region ItemImage
        private string _ItemImage;
        public string ItemImage
        {
            get { return _ItemImage; }
            set { SetProperty(ref _ItemImage, value); }
        }
        #endregion

        public string FkCategory { get; set; }

        #region Value
        private double _Value = 0;
        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
        #endregion

        public long IsInPercent { get; set; }

        #region IsSelected
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        #endregion

    }
}
