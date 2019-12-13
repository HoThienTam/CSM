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
        private long _IsTakeAway;
        public long IsTakeAway
        {
            get { return _IsTakeAway; }
            set { SetProperty(ref _IsTakeAway, value); }
        }
        #endregion

        #region CustomerCount
        private long _CustomerCount;
        public long CustomerCount
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

        #region ItemCount
        private long _ItemCount;
        public long ItemCount
        {
            get { return _ItemCount; }
            set { SetProperty(ref _ItemCount, value); }
        }
        #endregion

        #region TableName
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { SetProperty(ref _TableName, value); }
        }
        #endregion

        public VisualInvoiceModel()
        {
            this.Id = Guid.NewGuid().ToString();
            this.TableName = "CHỌN BÀN";
            this.CustomerCount = 1;
            this.ListItemInBill = new ObservableCollection<VisualItemMenuModel>();
            this.ListDiscount = new ObservableCollection<VisualItemMenuModel>();
        }
        public VisualInvoiceModel(VisualInvoiceModel obj)
        {
            this.Id = obj.Id;
            this.IsTakeAway = obj.IsTakeAway;
            this.CustomerCount = obj.CustomerCount;
            this.FkTable = obj.FkTable;
            this.TotalPrice = obj.TotalPrice;
            this.CreationDate = obj.CreationDate;
            this.OriginalPrice = obj.OriginalPrice;
            this.ListItemInBill = obj.ListItemInBill;
            this.ListDiscount = obj.ListDiscount;
            this.ItemCount = obj.ItemCount;
            this.TableName = obj.TableName;
        }
    }
}
