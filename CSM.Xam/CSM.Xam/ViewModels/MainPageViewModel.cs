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
using Telerik.XamarinForms.DataControls.ListView.Commands;
using CSM.Logic.Enums;
using Xamarin.Forms;
using Menu = CSM.EFCore.Menu;

namespace CSM.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public MainPageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            MessagingCenter.Subscribe<Invoice>(this, Messages.INVOICE_MESSAGE, ReceiveInvoiceMessageHandler);
            MessagingCenter.Subscribe<Zone>(this, Messages.ZONE_MESSAGE, ReceiveZoneMessageHandler);
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

        #region IsVisibleListCategoryBindProp
        private bool _IsVisibleListCategoryBindProp = true;
        public bool IsVisibleListCategoryBindProp
        {
            get { return _IsVisibleListCategoryBindProp; }
            set { SetProperty(ref _IsVisibleListCategoryBindProp, value); }
        }
        #endregion

        #region IsVisibleListItemBindProp
        private bool _IsVisibleListItemBindProp = false;
        public bool IsVisibleListItemBindProp
        {
            get { return _IsVisibleListItemBindProp; }
            set { SetProperty(ref _IsVisibleListItemBindProp, value); }
        }
        #endregion

        #region IsVisiblePopupBindProp
        private bool _IsVisiblePopupBindProp = false;
        public bool IsVisiblePopupBindProp
        {
            get { return _IsVisiblePopupBindProp; }
            set { SetProperty(ref _IsVisiblePopupBindProp, value); }
        }
        #endregion

        #endregion

        //Menu duoi

        #region ListMenuBindProp
        private ObservableCollection<Menu> _ListMenuBindProp = null;
        public ObservableCollection<Menu> ListMenuBindProp
        {
            get { return _ListMenuBindProp; }
            set { SetProperty(ref _ListMenuBindProp, value); }
        }
        #endregion

        //Frame hoa don

        #region ListSectionBindProp
        private ObservableCollection<Zone> _ListSectionBindProp = null;
        public ObservableCollection<Zone> ListSectionBindProp
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

        // Frame thu vien

        #region ListCategoryBindProp
        private ObservableCollection<Category> _ListCategoryBindProp = null;
        public ObservableCollection<Category> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region ListAllItemBindProp
        private List<Item> _ListAllItemBindProp = null;
        public List<Item> ListAllItemBindProp
        {
            get { return _ListAllItemBindProp; }
            set { SetProperty(ref _ListAllItemBindProp, value); }
        }
        #endregion

        #region ListItemBindProp
        private ObservableCollection<Item> _ListItemBindProp = null;
        public ObservableCollection<Item> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
        }
        #endregion

        //Frame bill

        #region ListItemInBillBindProp
        private ObservableCollection<Item> _ListItemInBillBindProp = null;
        public ObservableCollection<Item> ListItemInBillBindProp
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

        #region Command

        // Frame Thu vien

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
                    ListItemInBillBindProp = new ObservableCollection<Item>();
                }
                // Thuc hien cong viec tai day
                if (obj is Item item)
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

        #region TapCategoryCommand

        public DelegateCommand<object> TapCategoryCommand { get; private set; }
        private async void OnTapCategory(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var selectedCategory = (obj as ItemTapCommandContext).Item as Category;

                var listItem = ListAllItemBindProp.Where(h => h.FkCategory == selectedCategory.Id).ToList();
                ListItemBindProp = new ObservableCollection<Item>(listItem);
                IsVisibleListItemBindProp = true;
                IsVisibleListCategoryBindProp = false;
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
        private void InitTapCategoryCommand()
        {
            TapCategoryCommand = new DelegateCommand<object>(OnTapCategory);
            TapCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region GoBackCategoryCommand

        public DelegateCommand<object> GoBackCategoryCommand { get; private set; }
        private async void OnGoBackCategory(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                IsVisibleListCategoryBindProp = true;
                IsVisibleListItemBindProp = false;
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
        private void InitGoBackCategoryCommand()
        {
            GoBackCategoryCommand = new DelegateCommand<object>(OnGoBackCategory);
            GoBackCategoryCommand.ObservesCanExecute(() => IsNotBusy);
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
                        case "hamburger":
                            IsVisiblePopupBindProp = true;
                            break;
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
                            if (ListMenuBindProp == null)
                            {
                                ListMenuBindProp = new ObservableCollection<Menu>();
                            }
                            var menuLogic = new MenuLogic(_dbContext);
                            var menu = await menuLogic.CreateAsync(new Menu
                            {
                                Id = Guid.NewGuid().ToString(),
                                MenuName = "Menu01"
                            });
                            ListMenuBindProp.Add(menu);
                            break;
                        case "hoantat":
                            IsVisibleFrameMenuBindProp = false;
                            IsVisibleFrameBillBindProp = true;
                            break;
                        case "hoatdong":
                            break;
                        case "thongke":
                            break;
                        case "hanghoa":
                            break;
                        case "nhanvien":
                            break;
                        case "caidat":
                            IsVisiblePopupBindProp = false;
                            await NavigationService.NavigateAsync(nameof(CSM_08Page));
                            break;
                        case "phienlamviec":
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

        //Frame bill

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
                var invoiceLogic = new InvoiceLogic(_dbContext);
                var invoiceItemLogic = new InvoiceItemLogic(_dbContext);

                var invoice = new Invoice
                {
                    Id = Guid.NewGuid().ToString(),
                    TotalPrice = TotalPriceBindProp,
                    Status = (int)InvoiceStatus.Normal,
                    CreationDate = DateTime.Now.ToString("HH:mm:ss"),
                    PaidAmount = 0,
                    Tip = 0
                };

                await invoiceLogic.CreateAsync(invoice, false);
                foreach (var item in ListItemInBillBindProp)
                {
                    await invoiceItemLogic.CreateAsync(new InvoiceItem
                    {
                        FkInvoice = invoice.Id,
                        FkItem = item.Id,
                        Id = Guid.NewGuid().ToString(),
                        Quantity = 1
                    }, false);
                }
                await _dbContext.SaveChangesAsync();

                SetFramesInvisible();
                IsVisibleFrameHoaDonBindProp = true;
                ListItemInBillBindProp = null;
                TotalPriceBindProp = 0;
                ItemCountBindProp = 0;

                MessagingCenter.Send(invoice, Messages.INVOICE_MESSAGE);
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

        // Menu duoi


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
            var invoiceLogic = new InvoiceLogic(_dbContext);
            var listInvoice = await invoiceLogic.GetAllAsync();

            ListInvoiceBindProp = new ObservableCollection<Invoice>(listInvoice);
        }

        private async void GetAllZone()
        {
            var zoneLogic = new ZoneLogic(_dbContext);
            var listZone = await zoneLogic.GetAllAsync();

            ListSectionBindProp = new ObservableCollection<Zone>(listZone);
        }
        #endregion

        #region MessageHandler
        private void ReceiveInvoiceMessageHandler(Invoice invoice)
        {
            ListInvoiceBindProp.Add(invoice);
        }
        private void ReceiveZoneMessageHandler(Zone zone)
        {
            ListSectionBindProp.Add(zone);
        }
        #endregion

        #region Navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey(Keys.ITEM))
                    {
                        var item = parameters[Keys.ITEM] as Item;

                        ListItemBindProp.Add(new Item
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Price = item.Price,
                        });
                    }
                    break;
                case NavigationMode.New:
                    var itemLogic = new ItemLogic(_dbContext);
                    var categorylogic = new CategoryLogic(_dbContext);

                    var listItem = await itemLogic.GetAllAsync();
                    var listCategory = await categorylogic.GetAllAsync();

                    GetAllInvoice();
                    GetAllZone();

                    ListItemBindProp = new ObservableCollection<Item>(listItem);
                    ListAllItemBindProp = new List<Item>(listItem);

                    ListCategoryBindProp = new ObservableCollection<Category>(listCategory);
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
