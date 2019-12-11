using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualInvoiceModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region IsTakeAway
        private int _IsTakeAway;
        public int IsTakeAway
        {
            get { return _IsTakeAway; }
            set { SetProperty(ref _IsTakeAway, value); }
        }
        #endregion

        #region CustomerCount
        private int _CustomerCount;
        public int CustomerCount
        {
            get { return _CustomerCount; }
            set { SetProperty(ref _CustomerCount, value); }
        }
        #endregion

        #region FkTable
        private string _FkTable;
        public string FkTable
        {
            get { return _FkTable; }
            set { SetProperty(ref _FkTable, value); }
        }
        #endregion

        #region TotalPrice
        private double _TotalPrice;
        public double TotalPrice
        {
            get { return _TotalPrice; }
            set { SetProperty(ref _TotalPrice, value); }
        }
        #endregion

        #region CreationDate
        private string _CreationDate;
        public string CreationDate
        {
            get { return _CreationDate; }
            set { SetProperty(ref _CreationDate, value); }
        }
        #endregion

        public double OriginalPrice { get; set; }

        #region ListItemInBill
        private ObservableCollection<VisualItemMenuModel> _ListItemInBill;
        public ObservableCollection<VisualItemMenuModel> ListItemInBill
        {
            get { return _ListItemInBill; }
            set { SetProperty(ref _ListItemInBill, value); }
        }
        #endregion

        #region ListDiscount
        private ObservableCollection<VisualItemMenuModel> _ListDiscount;
        public ObservableCollection<VisualItemMenuModel> ListDiscount
        {
            get { return _ListDiscount; }
            set { SetProperty(ref _ListDiscount, value); }
        }
        #endregion

    }
}
