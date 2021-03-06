﻿using CSM.Logic.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualItemMenuModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

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

        #region FkCategory
        private string _FkCategory;
        public string FkCategory
        {
            get { return _FkCategory; }
            set { SetProperty(ref _FkCategory, value); }
        }
        #endregion

        #region Value
        private double _Value = 0;
        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
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

        #region IsDiscount
        private bool _IsDiscount;
        public bool IsDiscount
        {
            get { return _IsDiscount; }
            set { SetProperty(ref _IsDiscount, value); }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
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

        #region Status
        private Status _Status;
        public Status Status
        {
            get { return _Status; }
            set { SetProperty(ref _Status, value); }
        }
        #endregion

        #region Quantity
        private long _Quantity;
        public long Quantity
        {
            get { return _Quantity; }
            set { SetProperty(ref _Quantity, value); }
        }
        #endregion

        #region ListSubItem
        private ObservableCollection<VisualItemMenuModel> _ListSubItem = new ObservableCollection<VisualItemMenuModel>();
        public ObservableCollection<VisualItemMenuModel> ListSubItem
        {
            get { return _ListSubItem; }
            set { SetProperty(ref _ListSubItem, value); }
        }
        #endregion

    }
}
