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
using CSM.Logic.Enums;
using Xamarin.Forms;
using Menu = CSM.EFCore.Menu;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Telerik.XamarinForms.DataControls.ListView;

namespace CSM.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        private Dictionary<string, List<VisualTableModel>> _tableInZone;
        private Dictionary<string, List<Item>> _itemInMenu;
        private string _menuId;
        private string _selectedCategory;
        private VisualTableModel _oldTable;
        public MainPageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            MessagingCenter.Subscribe<Invoice>(this, Messages.INVOICE_MESSAGE, ReceiveInvoiceMessageHandler);
            MessagingCenter.Subscribe<ObservableCollection<VisualTableModel>>(this, Messages.TABLE_MESSAGE, ReceiveTableMessageHandler);
            MessagingCenter.Subscribe<VisualZoneModel>(this, Messages.ZONE_MESSAGE, ReceiveZoneMessageHandler);
        }

        #region Bind Property

        #region Visible Property

        #region IsVisibleFrameHoaDonKhuVucBindProp
        private bool _IsVisibleFrameHoaDonKhuVucBindProp = true;
        public bool IsVisibleFrameHoaDonKhuVucBindProp
        {
            get { return _IsVisibleFrameHoaDonKhuVucBindProp; }
            set { SetProperty(ref _IsVisibleFrameHoaDonKhuVucBindProp, value); }
        }
        #endregion

        #region IsVisibleFrameHoaDonBindProp
        private bool _IsVisibleFrameHoaDonBindProp = true;
        public bool IsVisibleFrameHoaDonBindProp
        {
            get { return _IsVisibleFrameHoaDonBindProp; }
            set
            {
                SetProperty(ref _IsVisibleFrameHoaDonBindProp, value);
                RaisePropertyChanged(nameof(IsVisbleFrameKhuVucBindProp));
            }
        }
        public bool IsVisbleFrameKhuVucBindProp
        {
            get { return !_IsVisibleFrameHoaDonBindProp; }
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

        #region IsVisibleListCategoryBindProp
        private bool _IsVisibleListCategoryBindProp = true;
        public bool IsVisibleListCategoryBindProp
        {
            get { return _IsVisibleListCategoryBindProp; }
            set
            {
                SetProperty(ref _IsVisibleListCategoryBindProp, value);
                RaisePropertyChanged(nameof(IsVisibleListItemBindProp));
            }
        }

        public bool IsVisibleListItemBindProp
        {
            get { return !_IsVisibleListCategoryBindProp; }
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
        private ObservableCollection<VisualMenuModel> _ListMenuBindProp = null;
        public ObservableCollection<VisualMenuModel> ListMenuBindProp
        {
            get { return _ListMenuBindProp; }
            set { SetProperty(ref _ListMenuBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                SetProperty(ref _IsEditing, value);
                RaisePropertyChanged(nameof(IsNotEditting));
            }
        }

        public bool IsNotEditting { get { return !_IsEditing; } }
        #endregion

        //Frame hoa don - Khu vuc

        #region ListSectionBindProp
        private ObservableCollection<VisualZoneModel> _ListSectionBindProp = null;
        public ObservableCollection<VisualZoneModel> ListSectionBindProp
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

        #region ListTableBindProp
        private ObservableCollection<VisualTableModel> _ListTableBindProp = null;
        public ObservableCollection<VisualTableModel> ListTableBindProp
        {
            get { return _ListTableBindProp; }
            set { SetProperty(ref _ListTableBindProp, value); }
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

        #region ListItemBindProp
        private ObservableCollection<VisualItemMenuModel> _ListItemBindProp = null;
        public ObservableCollection<VisualItemMenuModel> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
        }
        #endregion

        #region ListDiscountBindProp
        private ObservableCollection<VisualItemMenuModel> _ListDiscountBindProp = null;
        public ObservableCollection<VisualItemMenuModel> ListDiscountBindProp
        {
            get { return _ListDiscountBindProp; }
            set { SetProperty(ref _ListDiscountBindProp, value); }
        }
        #endregion

        #region ItemsFilterDescriptor
        private ObservableCollection<FilterDescriptorBase> _ItemsFilterDescriptor;
        public ObservableCollection<FilterDescriptorBase> ItemsFilterDescriptor
        {
            get { return _ItemsFilterDescriptor; }
            set { SetProperty(ref _ItemsFilterDescriptor, value); }
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

        #region TableBindProp
        private Tuple<string, string> _TableBindProp = new Tuple<string, string>("CHỌN BÀN", "");
        public Tuple<string, string> TableBindProp
        {
            get { return _TableBindProp; }
            set { SetProperty(ref _TableBindProp, value); }
        }
        #endregion

        // Frame menu

        #region ListItemInMenuBindProp
        private ObservableCollection<Item> _ListItemInMenuBindProp;
        public ObservableCollection<Item> ListItemInMenuBindProp
        {
            get { return _ListItemInMenuBindProp; }
            set { SetProperty(ref _ListItemInMenuBindProp, value); }
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
                if (obj is ItemTapCommandContext itemTap)
                {
                    _selectedCategory = (itemTap.Item as Category).Id;
                    Title = (itemTap.Item as Category).CategoryName;
                }
                if (obj is string category)
                {
                    switch (category)
                    {
                        case "discount":
                            _selectedCategory = "discount";
                            Title = "Giảm  giá";
                            break;
                        case "allitem":
                            _selectedCategory = "allitem";
                            Title = "Tất cả mặt hàng";
                            break;
                    }
                }
                ItemsFilterDescriptor.Clear();
                ItemsFilterDescriptor.Add(new DelegateFilterDescriptor { Filter = FilterByCategory });
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
                            await NavigationService.NavigateAsync(nameof(CSM_03Page));
                            break;
                        case "danhmuc":

                            var param = new NavigationParameters();
                            param.Add(Keys.IS_EDITING, true); //Chinh sua

                            await NavigationService.NavigateAsync(nameof(CSM_02_01Page), param);
                            break;
                        case "thucdon":
                            var menuLogic = new MenuLogic(_dbContext);
                            var menu = await menuLogic.CreateAsync(new Menu
                            {
                                Id = Guid.NewGuid().ToString(),
                                MenuName = "Menu01",
                                ImageIcon = "\uf0f4",
                            });
                            var visualMenu = Mapper.Map<VisualMenuModel>(menu);
                            ListMenuBindProp.Add(visualMenu);
                            break;
                        case "hoantat":
                            IsEditing = false;
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
                var tableLogic = new TableLogic(_dbContext);

                var invoice = new Invoice
                {
                    Id = Guid.NewGuid().ToString(),
                    TotalPrice = TotalPriceBindProp,
                    Status = (int)InvoiceStatus.Normal,
                    CreationDate = DateTime.Now.ToString("HH:mm:ss"),
                    PaidAmount = 0,
                    Tip = 0,
                    FkTable = TableBindProp.Item2,
                    CustomerCount = 1
                };

                await tableLogic.ChangeStatusAsync(new Table
                {
                    Id = TableBindProp.Item2,
                    IsSelected = 1
                }, false);

                await invoiceLogic.CreateAsync(invoice, false);
                foreach (var item in ListItemInBillBindProp)
                {
                    await invoiceItemLogic.CreateAsync(new InvoiceItem
                    {
                        FkInvoice = invoice.Id,
                        FkItem = item.Id,
                        Quantity = 1
                    }, false);
                }
                await _dbContext.SaveChangesAsync();

                IsVisibleFrameHoaDonKhuVucBindProp = true;

                IsVisibleFrameThuVienBindProp = false;
                IsVisibleFrameThucDonBindProp = false;
                IsVisibleFrameBillBindProp = false;

                //reset bill
                ListItemInBillBindProp = null;
                TotalPriceBindProp = 0;
                ItemCountBindProp = 0;
                _oldTable = null;

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

        #region SelectMenuCommand

        public DelegateCommand<object> SelectMenuCommand { get; private set; }
        private async void OnSelectMenu(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day    
                if (IsEditing)
                {
                    IsVisibleFrameThuVienBindProp = false;

                    IsVisibleFrameThucDonBindProp = true;
                }
                else
                {
                    IsVisibleFrameHoaDonKhuVucBindProp = false;
                    IsVisibleFrameThuVienBindProp = false;

                    IsVisibleFrameThucDonBindProp = true;
                    IsVisibleFrameBillBindProp = true;
                }
                var menu = obj as VisualMenuModel;

                _menuId = menu.Id;

                var listItem = new List<Item>();
                _itemInMenu.TryGetValue(menu.Id, out listItem);
                ListItemInMenuBindProp = new ObservableCollection<Item>(listItem);
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
        private void InitSelectMenuCommand()
        {
            SelectMenuCommand = new DelegateCommand<object>(OnSelectMenu);
            SelectMenuCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region ModifyMenuCommand

        public DelegateCommand<object> ModifyMenuCommand { get; private set; }
        private bool CanExecuteModifyMenu(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (IsNotEditting)
            {
                return false;
            }
            return true;
        }
        private async void OnModifyMenu(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var menu = obj as VisualMenuModel;

                var param = new NavigationParameters();
                param.Add(Keys.MENU, menu);
                await NavigationService.NavigateAsync(nameof(CSM_01Page), param);
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
        private void InitModifyMenuCommand()
        {
            ModifyMenuCommand = new DelegateCommand<object>(OnModifyMenu, CanExecuteModifyMenu);
            ModifyMenuCommand.ObservesProperty(() => IsNotBusy);
            ModifyMenuCommand.ObservesProperty(() => IsNotEditting);
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
                            IsVisibleFrameBillBindProp = false;
                            IsVisibleFrameThuVienBindProp = false;
                            IsVisibleFrameThucDonBindProp = false;

                            IsVisibleFrameHoaDonKhuVucBindProp = true;
                            IsVisibleFrameHoaDonBindProp = true;
                            break;
                        case "thuvien":
                            if (IsNotEditting)
                            {
                                IsVisibleFrameHoaDonKhuVucBindProp = false;
                                IsVisibleFrameThucDonBindProp = false;

                                IsVisibleFrameThuVienBindProp = true;
                                IsVisibleFrameBillBindProp = true;
                                IsVisibleListCategoryBindProp = true;
                            }
                            else
                            {
                                IsVisibleFrameHoaDonKhuVucBindProp = false;
                                IsVisibleFrameThucDonBindProp = false;
                                IsVisibleFrameBillBindProp = false;

                                IsVisibleFrameThuVienBindProp = true;
                                IsVisibleListCategoryBindProp = true;
                            }
                            break;
                        case "chinhsua":
                            IsEditing = true;
                            IsVisibleFrameBillBindProp = false;
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

        // Frame Hoa don - Khu vuc

        #region SelectZoneCommand

        public DelegateCommand<object> SelectZoneCommand { get; private set; }
        private async void OnSelectZone(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is VisualZoneModel zone)
                {
                    IsVisibleFrameHoaDonBindProp = false;

                    var listTable = new List<VisualTableModel>();
                    _tableInZone.TryGetValue(zone.Id, out listTable);
                    ListTableBindProp = new ObservableCollection<VisualTableModel>(listTable);
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
        private void InitSelectZoneCommand()
        {
            SelectZoneCommand = new DelegateCommand<object>(OnSelectZone);
            SelectZoneCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectTableCommand

        public DelegateCommand<object> SelectTableCommand { get; private set; }
        private async void OnSelectTable(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is VisualTableModel table)
                {
                    var zone = ListSectionBindProp.FirstOrDefault(h => h.Id == table.FkZone);
                    // Luu ten ban va doi mau`
                    TableBindProp = new Tuple<string, string>($"{zone.ZoneName} - {table.TableName}", table.Id);
                    table.IsSelected = true;
                    // Luu gia tri ban da chon
                    if (_oldTable != null)
                    {
                        _oldTable.IsSelected = false;
                    }
                    _oldTable = table;

                    IsVisibleFrameThucDonBindProp = true;
                    IsVisibleFrameBillBindProp = true;

                    IsVisibleFrameHoaDonKhuVucBindProp = false;
                }
                else
                {
                    IsVisibleFrameThucDonBindProp = false;
                    IsVisibleFrameBillBindProp = false;
                    IsVisibleFrameThuVienBindProp = false;

                    IsVisibleFrameHoaDonKhuVucBindProp = true;

                    var zone = ListSectionBindProp.FirstOrDefault();

                    IsBusy = false;
                    OnSelectZone(zone);
                    IsBusy = true;
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
        private void InitSelectTableCommand()
        {
            SelectTableCommand = new DelegateCommand<object>(OnSelectTable);
            SelectTableCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        // Frame menu

        #region AddItemToMenuCommand

        public DelegateCommand<object> AddItemToMenuCommand { get; private set; }
        private async void OnAddItemToMenu(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var param = new NavigationParameters();
                param.Add(Keys.LIST_CATEGORY, ListCategoryBindProp);
                param.Add(Keys.LIST_ITEM, ListItemBindProp);
                param.Add(Keys.ZONE, _menuId);
                await NavigationService.NavigateAsync(nameof(CSM_11Page), param);
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
        private void InitAddItemToMenuCommand()
        {
            AddItemToMenuCommand = new DelegateCommand<object>(OnAddItemToMenu);
            AddItemToMenuCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion

        #region Method

        #region FilterByCategory
        private bool FilterByCategory(object item)
        {
            var itemModel = (VisualItemMenuModel)item;
            if (_selectedCategory.Equals("allitem"))
            {
                return !string.IsNullOrWhiteSpace(itemModel.FkCategory);
            }
            else if (_selectedCategory.Equals("discount"))
            {
                return string.IsNullOrWhiteSpace(itemModel.FkCategory);
            }
            else
            {
                return itemModel.FkCategory == _selectedCategory;
            }
        }
        #endregion

        private async void GetAllInvoice()
        {
            var invoiceLogic = new InvoiceLogic(_dbContext);
            var listInvoice = await invoiceLogic.GetAllAsync();

            ListInvoiceBindProp = new ObservableCollection<Invoice>(listInvoice);
        }

        private async void GetAllZone()
        {
            _tableInZone = new Dictionary<string, List<VisualTableModel>>();
            var zoneLogic = new ZoneLogic(_dbContext);
            var tableLogic = new TableLogic(_dbContext);

            var listZone = await zoneLogic.GetAllAsync();
            var listVisualZone = Mapper.Map<List<VisualZoneModel>>(listZone);
            // lay danh sach ban trong tung khu vuc
            foreach (var zone in listZone)
            {
                var listTable = await tableLogic.GetTableByZoneAsync(zone.Id);
                var listVisualTable = Mapper.Map<List<VisualTableModel>>(listTable);
                _tableInZone.Add(zone.Id, listVisualTable);
            }

            ListSectionBindProp = new ObservableCollection<VisualZoneModel>(listVisualZone);
        }

        private async void GetAllMenuItem()
        {
            //lay danh sach category va item va discount
            var itemLogic = new ItemLogic(_dbContext);
            var discountLogic = new DiscountLogic(_dbContext);
            var categorylogic = new CategoryLogic(_dbContext);

            var listItem = await itemLogic.GetAllAsync();
            var listCategory = await categorylogic.GetAllAsync();
            var listDiscount = await discountLogic.GetAllAsync();

            // Mapping data
            var listVisualItem = Mapper.Map<List<VisualItemMenuModel>>(listItem);
            var listVisualDiscount = Mapper.Map<List<VisualItemMenuModel>>(listDiscount);
            //Gop 2 list
            listVisualItem.AddRange(listVisualDiscount);

            ListItemBindProp = new ObservableCollection<VisualItemMenuModel>(listVisualItem);

            ListCategoryBindProp = new ObservableCollection<Category>(listCategory);

            // lay danh sach menu va item trong tung menu
            var menuLogic = new MenuLogic(_dbContext);
            var menuItemLogic = new MenuItemLogic(_dbContext);

            var listMenu = await menuLogic.GetAllAsync();
            var listVisualMenu = Mapper.Map<List<VisualMenuModel>>(listMenu);

            _itemInMenu = new Dictionary<string, List<Item>>();
            //Lay danh sach item trong tung menu
            foreach (var menu in listMenu)
            {
                var listItemIdInMenu = await menuItemLogic.GetAsync(menu.Id);
                List<Item> listItemInMenu = new List<Item>();
                //lay thong tin tung item
                foreach (var item in listItemIdInMenu)
                {
                    listItemInMenu.Add(listItem.FirstOrDefault(h => h.Id == item.FkItem));
                }
                _itemInMenu.Add(menu.Id, listItemInMenu);
            }

            if (listVisualMenu.Count == 0)
            {
                var menu = await menuLogic.CreateAsync(new Menu
                {
                    Id = Guid.NewGuid().ToString(),
                    MenuName = "Menu01",
                    ImageIcon = "\uf0f4",
                });
                var visualMenu = Mapper.Map<VisualMenuModel>(menu);
                listVisualMenu.Add(visualMenu);
            }
            ListMenuBindProp = new ObservableCollection<VisualMenuModel>(listVisualMenu);

        }

        #endregion

        #region MessageHandler
        private void ReceiveInvoiceMessageHandler(Invoice invoice)
        {
            ListInvoiceBindProp.Add(invoice);
        }
        private void ReceiveTableMessageHandler(ObservableCollection<VisualTableModel> tableCollection)
        {
            var zoneDb = ListSectionBindProp.FirstOrDefault(h => h.Id == tableCollection[0].FkZone);
            _tableInZone[zoneDb.Id] = new List<VisualTableModel>(tableCollection);
        }
        private void ReceiveZoneMessageHandler(VisualZoneModel zone)
        {
            var zoneDb = ListSectionBindProp.FirstOrDefault(h => h.Id == zone.Id);
            if (zoneDb != null)
            {
                if (zone.IsDeleted)
                {
                    ListSectionBindProp.Remove(zoneDb);
                    _tableInZone.Remove(zoneDb.Id);
                }
                else
                {
                    zoneDb.ZoneName = zone.ZoneName;
                }
            }
            else
            {
                ListSectionBindProp.Add(zone);
                _tableInZone.Add(zone.Id, new List<VisualTableModel>());
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
                    if (parameters.ContainsKey(Keys.ITEM))
                    {
                        var item = parameters[Keys.ITEM] as Item;

                        ListItemBindProp.Add(new VisualItemMenuModel
                        {
                            Id = item.Id,
                            Name = item.ItemName,
                            Value = item.Price,
                            FkCategory = item.FkCategory,
                        });
                    }
                    if (parameters.ContainsKey(Keys.LIST_ITEM))
                    {
                        var listItems = parameters[Keys.LIST_ITEM] as List<Item>;

                        foreach (var item in listItems)
                        {
                            if (!ListItemInMenuBindProp.Any(h => h.Id == item.Id))
                            {
                                ListItemInMenuBindProp.Add(item);
                                _itemInMenu[_menuId].Add(item);
                            }
                        }
                    }
                    if (parameters.ContainsKey(Keys.MENU))
                    {
                        var menu = parameters[Keys.MENU] as VisualMenuModel;
                        ListMenuBindProp.Remove(menu);
                    }
                    break;
                case NavigationMode.New:

                    GetAllInvoice();
                    GetAllZone();
                    GetAllMenuItem();
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
