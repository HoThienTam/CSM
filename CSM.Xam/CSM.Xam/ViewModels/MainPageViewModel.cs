﻿using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CSM.EFCore;
using CSM.Logic;
using System.Linq;
using CSM.Logic;
using CSM.Xam.Views;

namespace CSM.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public dataContext dbContext = new dataContext((App.Current as App).DbConnectionString);
        public MainPageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

        }

        #region Bind Property
        
        #region Visible Property

        #region IsVisibleFrameHoaDonBindProp
        private bool _IsVisibleFrameHoaDonBindProp = true;
        public bool IsVisibleFrameHoaDonBindProp
        {
            get { return _IsVisibleFrameHoaDonBindProp; }
            set { SetProperty(ref _IsVisibleFrameHoaDonBindProp, value); }
        }
        #endregion

        #region IsVisibleFrameThuVienBindProp
        private bool _IsVisibleFrameThuVienBindProp = false;
        public bool IsVisibleFrameThuVienBindProp
        {
            get { return _IsVisibleFrameThuVienBindProp; }
            set { SetProperty(ref _IsVisibleFrameThuVienBindProp, value); }
        }
        #endregion

        #region IsVisibleFrameMenuBindProp
        private bool _IsVisibleFrameMenuBindProp = false;
        public bool IsVisibleFrameMenuBindProp
        {
            get { return _IsVisibleFrameMenuBindProp; }
            set { SetProperty(ref _IsVisibleFrameMenuBindProp, value); }
        }
        #endregion

        #region IsVisibleFrameBillBindProp
        private bool _IsVisibleFrameBillBindProp = false;
        public bool IsVisibleFrameBillBindProp
        {
            get { return _IsVisibleFrameBillBindProp; }
            set { SetProperty(ref _IsVisibleFrameBillBindProp, value); }
        }
        #endregion

        #region IsVisibleFrameThucDonBindProp
        private bool _IsVisibleFrameThucDonBindProp = false;
        public bool IsVisibleFrameThucDonBindProp
        {
            get { return _IsVisibleFrameThucDonBindProp; }
            set { SetProperty(ref _IsVisibleFrameThucDonBindProp, value); }
        }
        #endregion

        #region IsVisbleFrameKhuVucBindProp
        private bool _IsVisbleFrameKhuVucBindProp = false;
        public bool IsVisbleFrameKhuVucBindProp
        {
            get { return _IsVisbleFrameKhuVucBindProp; }
            set { SetProperty(ref _IsVisbleFrameKhuVucBindProp, value); }
        }
        #endregion

        #endregion

        #region ListMenuBindProp
        private ObservableCollection<string> _ListMenuBindProp = null;
        public ObservableCollection<string> ListMenuBindProp
        {
            get { return _ListMenuBindProp; }
            set { SetProperty(ref _ListMenuBindProp, value); }
        }
        #endregion

        #region ListSectionBindProp
        private ObservableCollection<string> _ListSectionBindProp = null;
        public ObservableCollection<string> ListSectionBindProp
        {
            get { return _ListSectionBindProp; }
            set { SetProperty(ref _ListSectionBindProp, value); }
        }
        #endregion

        #region ListInvoiceBindProp
        private ObservableCollection<Invoice> _ListInvoiceBindProp = null;
        public ObservableCollection<Invoice> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        #region ListCategoryBindProp
        private ObservableCollection<Category> _ListCategoryBindProp = null;
        public ObservableCollection<Category> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region ListItemBindProp
        private ObservableCollection<ItemExtended> _ListItemBindProp = null;
        public ObservableCollection<ItemExtended> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
        }
        #endregion

        #region Bill Property

        #region ListItemInBillBindProp
        private ObservableCollection<ItemExtended> _ListItemInBillBindProp = null;
        public ObservableCollection<ItemExtended> ListItemInBillBindProp
        {
            get { return _ListItemInBillBindProp; }
            set { SetProperty(ref _ListItemInBillBindProp, value); }
        }
        #endregion

        #region ItemCountBindProp
        private int _ItemCountBindProp = 0;
        public int ItemCountBindProp
        {
            get { return _ItemCountBindProp; }
            set { SetProperty(ref _ItemCountBindProp, value); }
        }
        #endregion

        #region TotalPriceBindProp
        private double _TotalPriceBindProp = 0;
        public double TotalPriceBindProp
        {
            get { return _TotalPriceBindProp; }
            set { SetProperty(ref _TotalPriceBindProp, value); }
        }
        #endregion

        #endregion

        #endregion

        #region Command

        #region SelectItemCommand

        public DelegateCommand<object> SelectItemCommand { get; private set; }
        private async void OnSelectItem(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if (ListItemInBillBindProp == null)
                {
                    ListItemInBillBindProp = new ObservableCollection<ItemExtended>();
                }
                // Thuc hien cong viec tai day
                if (obj is ItemExtended item)
                {
                    ListItemInBillBindProp.Add(item);
                    ItemCountBindProp++;
                    TotalPriceBindProp += item.Price;
                }
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitSelectItemCommand()
        {
            SelectItemCommand = new DelegateCommand<object>(OnSelectItem);
            SelectItemCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region TapMenuCommand

        public DelegateCommand<object> TapMenuCommand { get; private set; }
        private async void OnTapMenu(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is string menu)
                {
                    switch (menu)
                    {
                        case "hoadon":
                            SetFramesInvisible();
                            IsVisibleFrameHoaDonBindProp = true;
                            break;
                        case "thuvien":
                            SetFramesInvisible();
                            IsVisibleFrameThuVienBindProp = true;
                            IsVisibleFrameBillBindProp = true;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitTapMenuCommand()
        {
            TapMenuCommand = new DelegateCommand<object>(OnTapMenu);
            TapMenuCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region LuuHoaDonCommand

        public DelegateCommand<object> LuuHoaDonCommand { get; private set; }
        private async void OnLuuHoaDon(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var invoiceLogic = new InvoiceLogic(dbContext);
                await invoiceLogic.CreateAsync(new Invoice
                {
                    TotalPrice = TotalPriceBindProp,
                });
                SetFramesInvisible();
                IsVisibleFrameHoaDonBindProp = true;
                ListItemInBillBindProp = null;
                GetAllInvoice();
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitLuuHoaDonCommand()
        {
            LuuHoaDonCommand = new DelegateCommand<object>(OnLuuHoaDon);
            LuuHoaDonCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region ThanhToanCommand

        public DelegateCommand<object> ThanhToanCommand { get; private set; }
        private async void OnThanhToan(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                await NavigationService.NavigateAsync(nameof(CSM_10Page));
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitThanhToanCommand()
        {
            ThanhToanCommand = new DelegateCommand<object>(OnThanhToan);
            ThanhToanCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion

        #region Method
        private void SetFramesInvisible()
        {
            IsVisibleFrameBillBindProp = false;
            IsVisbleFrameKhuVucBindProp = false;
            IsVisibleFrameHoaDonBindProp = false;
            IsVisibleFrameMenuBindProp = false;
            IsVisibleFrameThucDonBindProp = false;
            IsVisibleFrameThuVienBindProp = false;
        }

        private async void GetAllInvoice()
        {
            var invoiceLogic = new InvoiceLogic(dbContext);
            var listInvoice = await invoiceLogic.GetAllAsync();

            ListInvoiceBindProp = new ObservableCollection<Invoice>();
            foreach (var invoice in listInvoice)
            {
                ListInvoiceBindProp.Add(invoice);
            }
        }
        #endregion

        #region Navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    var itemLogic = new ItemLogic(dbContext);

                    var listItem = await itemLogic.GetAllAsync(null);
                    ListItemBindProp = new ObservableCollection<ItemExtended>();
                    foreach (var item in listItem)
                    {
                        ListItemBindProp.Add(new ItemExtended
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Price = item.Price,
                        });
                    }
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
