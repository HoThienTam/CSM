using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualItemMenuModel : BindableBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ItemImage { get; set; }
        public string FkCategory { get; set; }
        public double Value { get; set; }
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
