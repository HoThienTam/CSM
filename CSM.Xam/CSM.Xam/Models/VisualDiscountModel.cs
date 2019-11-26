using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualDiscountModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region DiscountName
        private string _DiscountName;
        public string DiscountName
        {
            get { return _DiscountName; }
            set { SetProperty(ref _DiscountName, value); }
        }
        #endregion

        #region DiscountType
        private int _DiscountType;
        public int DiscountType
        {
            get { return _DiscountType; }
            set { SetProperty(ref _DiscountType, value); }
        }
        #endregion

        #region DiscountValue
        private double _DiscountValue;
        public double DiscountValue
        {
            get { return _DiscountValue; }
            set { SetProperty(ref _DiscountValue, value); }
        }
        #endregion

        #region MaxValue
        private double _MaxValue;
        public double MaxValue
        {
            get { return _MaxValue; }
            set { SetProperty(ref _MaxValue, value); }
        }
        #endregion

        #region IsInPercent
        private bool _IsInPercent;
        public bool IsInPercent
        {
            get { return _IsInPercent; }
            set { SetProperty(ref _IsInPercent, value); }
        }
        #endregion

    }
}
