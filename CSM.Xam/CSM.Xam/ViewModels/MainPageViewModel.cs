using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CSM.EFCore;
using CSM.Logic;
using System.Linq;
using CSM.Xam.Views;

namespace CSM.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
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

        #region SelectViewCommand

        public DelegateCommand<object> SelectViewCommand { get; private set; }
        private async void OnSelectView(object obj)
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
                        case "chinhsua":
                            SetFramesInvisible();
                            IsVisibleFrameThuVienBindProp = true;
                            IsVisibleFrameMenuBindProp = true;
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
        private void InitSelectViewCommand()
        {
            SelectViewCommand = new DelegateCommand<object>(OnSelectView);
            SelectViewCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region NavigateCommand

        public DelegateCommand<object> NavigateCommand { get; private set; }
        private async void OnNavigate(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is string view)
                {
                    switch (view)
                    {
                        case "mathang":
                            await NavigationService.NavigateAsync(nameof(CSM_02Page));
                            break;
                        case "giamgia":
                            break;
                        case "danhmuc":

                            var param = new NavigationParameters();
                            param.Add(Keys.IS_EDITING, true); //Chinh sua

                            await NavigationService.NavigateAsync(nameof(CSM_02_01Page), param);
                            break;
                        case "thucdon":
                            break;
                        case "hoantat":
                            IsVisibleFrameMenuBindProp = false;
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
        private void InitNavigateCommand()
        {
            NavigateCommand = new DelegateCommand<object>(OnNavigate);
            NavigateCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region LuuHoaDonCommand

        public DelegateCommand<object> LuuHoaDonCommand { get; private set; }
        private bool CanExecuteLuuHoaDon(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (ListItemInBillBindProp == null)
            {
                return false;
            }
            return true;
        }
        private async void OnLuuHoaDon(object obj)
        {
           

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var invoiceLogic = new InvoiceLogic(Helper.GetDataContext());
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
            LuuHoaDonCommand = new DelegateCommand<object>(OnLuuHoaDon, CanExecuteLuuHoaDon);
            LuuHoaDonCommand.ObservesProperty(() => IsNotBusy);
            LuuHoaDonCommand.ObservesProperty(() => ListItemInBillBindProp);
        }

        #endregion

        #region ThanhToanCommand

        public DelegateCommand<object> ThanhToanCommand { get; private set; }
        private bool CanExecuteThanhToan(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (ListItemInBillBindProp == null)
            {
                return false;
            }
            return true;
        }
        private async void OnThanhToan(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day

                // Tao param de pass data sang trang khac
                var param = new NavigationParameters();
                param.Add(Keys.BILL, ListItemInBillBindProp);
                param.Add(Keys.TOTAL_PRICE, TotalPriceBindProp);

                await NavigationService.NavigateAsync(nameof(CSM_10Page), param);
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
            ThanhToanCommand = new DelegateCommand<object>(OnThanhToan, CanExecuteThanhToan);
            ThanhToanCommand.ObservesProperty(() => IsNotBusy);
            ThanhToanCommand.ObservesProperty(() => ListItemInBillBindProp);
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
            var invoiceLogic = new InvoiceLogic(Helper.GetDataContext());
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
                    var itemLogic = new ItemLogic(Helper.GetDataContext());

                    var listItem = await itemLogic.GetAllAsync();
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
