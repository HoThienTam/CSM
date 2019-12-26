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
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace CSM.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        HubConnection connection;
        private dataContext _dbContext = Helper.GetDataContext();
        private Dictionary<string, List<VisualTableModel>> _tableInZone;
        private Dictionary<string, List<VisualItemMenuModel>> _itemInMenu;
        private string _menuId;
        private VisualTableModel _oldTable;
        public MainPageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            CurrentBillBindProp = new VisualInvoiceModel();
            MessagingCenter.Subscribe<ObservableCollection<VisualTableModel>>(this, Messages.TABLE_MESSAGE, ReceiveTableMessageHandler);
            MessagingCenter.Subscribe<VisualZoneModel>(this, Messages.ZONE_MESSAGE, ReceiveZoneMessageHandler);
            MessagingCenter.Subscribe<VisualCategoryModel>(this, Messages.CATEGORY_MESSAGE, ReceiveCategoryMessageHandler);
            connection = new HubConnectionBuilder()
                   .WithUrl("http://8cd84a42.ngrok.io/messageHub")
                   .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<Invoice>("ReceiveInvoice", (invoice) =>
            {
                SaveInvoice(invoice);
            });
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

        #region IsVisibleFrameItemBindProp
        private bool _IsVisibleFrameItemBindProp = false;
        public bool IsVisibleFrameItemBindProp
        {
            get { return _IsVisibleFrameItemBindProp; }
            set { SetProperty(ref _IsVisibleFrameItemBindProp, value); }
        }
        #endregion

        #region IsVisibleBillModeBindProp
        private bool _IsVisibleBillModeBindProp = false;
        public bool IsVisibleBillModeBindProp
        {
            get { return _IsVisibleBillModeBindProp; }
            set { SetProperty(ref _IsVisibleBillModeBindProp, value); }
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
        private ObservableCollection<VisualInvoiceModel> _ListInvoiceBindProp = null;
        public ObservableCollection<VisualInvoiceModel> ListInvoiceBindProp
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

        #region IsDeleting
        private bool _IsDeleting = false;
        public bool IsDeleting
        {
            get { return _IsDeleting; }
            set 
            { 
                SetProperty(ref _IsDeleting, value);
                RaisePropertyChanged(nameof(IsNotDeleting));
            }
        }
        public bool IsNotDeleting { get { return !_IsDeleting; } }
        #endregion

        // Frame thu vien

        #region ListCategoryBindProp
        private ObservableCollection<VisualCategoryModel> _ListCategoryBindProp = null;
        public ObservableCollection<VisualCategoryModel> ListCategoryBindProp
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

        #region ListItem
        private List<VisualItemMenuModel> _ListItem = null;
        public List<VisualItemMenuModel> ListItem
        {
            get { return _ListItem; }
            set { SetProperty(ref _ListItem, value); }
        }
        #endregion

        //Frame bill

        #region CurrentBillBindProp
        private VisualInvoiceModel _CurrentBillBindProp = null;
        public VisualInvoiceModel CurrentBillBindProp
        {
            get { return _CurrentBillBindProp; }
            set { SetProperty(ref _CurrentBillBindProp, value); }
        }
        #endregion

        #region IsTakeAway
        private bool _IsTakeAway = false;
        public bool IsTakeAway
        {
            get { return _IsTakeAway; }
            set
            {
                SetProperty(ref _IsTakeAway, value);
                RaisePropertyChanged(nameof(IsInPlace));
            }
        }
        public bool IsInPlace { get { return !_IsTakeAway; } }
        #endregion

        #region IsEditingBill
        private bool _IsEditingBill = false;
        public bool IsEditingBill
        {
            get { return _IsEditingBill; }
            set { SetProperty(ref _IsEditingBill, value); }
        }
        #endregion

        // Frame menu

        #region ListItemInMenuBindProp
        private ObservableCollection<VisualItemMenuModel> _ListItemInMenuBindProp = null;
        public ObservableCollection<VisualItemMenuModel> ListItemInMenuBindProp
        {
            get { return _ListItemInMenuBindProp; }
            set { SetProperty(ref _ListItemInMenuBindProp, value); }
        }
        #endregion

        // Frame mat hang

        #region ListDiscountBindProp
        private ObservableCollection<VisualItemMenuModel> _ListDiscountBindProp = null;
        public ObservableCollection<VisualItemMenuModel> ListDiscountBindProp
        {
            get { return _ListDiscountBindProp; }
            set { SetProperty(ref _ListDiscountBindProp, value); }
        }
        #endregion

        #region SelectedItem
        private VisualItemMenuModel _SelectedItem = null;
        public VisualItemMenuModel SelectedItem
        {
            get { return _SelectedItem; }
            set { SetProperty(ref _SelectedItem, value); }
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
                // Thuc hien cong viec tai day
                if (obj is VisualItemMenuModel item)
                {
                    if (IsNotEditting)
                    {
                        if (item.IsDiscount)
                        {
                            if (CurrentBillBindProp.ListDiscount.Any(h => h.Id == item.Id))
                            {
                                return;
                            }
                            if (item.IsInPercent)
                            {
                                var discount = new VisualItemMenuModel
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Value = item.Value * CurrentBillBindProp.TotalPrice / 100,
                                    IsInPercent = item.IsInPercent,
                                    Status = Status.New,
                                    //Giu gia tri % lai
                                    Quantity = (int)item.Value
                                };
                                CurrentBillBindProp.ListDiscount.Add(discount);
                                CurrentBillBindProp.TotalPrice -= discount.Value;
                            }
                            else
                            {
                                var discount = new VisualItemMenuModel
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Value = item.Value,
                                    IsInPercent = item.IsInPercent,
                                    Status = Status.New,
                                    Quantity = 1
                                };
                                CurrentBillBindProp.ListDiscount.Add(discount);
                                CurrentBillBindProp.TotalPrice -= discount.Value;
                            }
                            if (CurrentBillBindProp.TotalPrice < 0)
                            {
                                CurrentBillBindProp.TotalPrice = 0;
                            }
                        }
                        else
                        {
                            IsVisibleFrameItemBindProp = true;
                            SelectedItem = new VisualItemMenuModel
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Value = item.Value,
                                Quantity = 0
                            };
                        }
                    }
                    else
                    {
                        var param = new NavigationParameters();
                        param.Add(Keys.ITEM, item);
                        await NavigationService.NavigateAsync(nameof(CSM_02Page), param);
                        IsVisibleListCategoryBindProp = true;
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
                string selectedCategory = "";
                if (obj is ItemTapCommandContext itemTap)
                {
                    selectedCategory = (itemTap.Item as VisualCategoryModel).Id;
                    Title = (itemTap.Item as VisualCategoryModel).CategoryName;
                }
                if (obj is string category)
                {
                    switch (category)
                    {
                        case "discount":
                            ListItemBindProp = ListDiscountBindProp;
                            Title = "Giảm giá";
                            break;
                        case "allitem":
                            var listItem = ListItem.Where(h => h.IsDiscount == false).ToList();
                            ListItemBindProp = new ObservableCollection<VisualItemMenuModel>(listItem);
                            Title = "Tất cả mặt hàng";
                            break;
                    }
                }
                else
                {
                    var listItem = ListItem.Where(h => h.FkCategory == selectedCategory).ToList();
                    ListItemBindProp = new ObservableCollection<VisualItemMenuModel>(listItem);
                }
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
                Title = "Thư viện";
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
                                MenuName = ListMenuBindProp.Count < 9 ? $"Menu0{ListMenuBindProp.Count + 1}" : $"Menu{ListMenuBindProp.Count + 1}",
                                ImageIcon = "\uf0f4",
                            });
                            var visualMenu = Mapper.Map<VisualMenuModel>(menu);
                            ListMenuBindProp.Add(visualMenu);
                            _itemInMenu.Add(visualMenu.Id, new List<VisualItemMenuModel>());
                            break;
                        case "hoantat":
                            IsEditing = false;
                            IsVisibleFrameBillBindProp = true;
                            break;
                        case "hoatdong":
                            IsVisiblePopupBindProp = false;
                            await NavigationService.NavigateAsync(nameof(CSM_04Page));
                            break;
                        case "thongke":
                            IsVisiblePopupBindProp = false;
                            await NavigationService.NavigateAsync(nameof(CSM_05Page));
                            break;
                        case "hanghoa":
                            IsVisiblePopupBindProp = false;
                            await NavigationService.NavigateAsync(nameof(CSM_06Page));
                            break;
                        case "nhanvien":
                            IsVisiblePopupBindProp = false;
                            await NavigationService.NavigateAsync(nameof(CSM_07Page));
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

        #region SaveInvoiceCommand

        public DelegateCommand<object> SaveInvoiceCommand { get; private set; }
        private bool CanExecuteSaveInvoice(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (CurrentBillBindProp.ListItemInBill.Count == 0)
            {
                return false;
            }
            return true;
        }
        private async void OnSaveInvoice(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var invoiceLogic = new InvoiceLogic(_dbContext);
                var invoiceItemLogic = new InvoiceItemLogic(_dbContext);
                var subItemLogic = new ItemDiscountLogic(_dbContext);
                var tableLogic = new TableLogic(_dbContext);

                if (string.IsNullOrWhiteSpace(CurrentBillBindProp.FkTable))
                {
                    if (CurrentBillBindProp.IsTakeAway == 0)
                    {
                        await PageDialogService.DisplayAlertAsync("", "Bạn chưa chọn bàn!", "OK");
                        return;
                    }
                }

                var invoice = new Invoice
                {
                    Id = CurrentBillBindProp.Id,
                    TotalPrice = CurrentBillBindProp.TotalPrice,
                    Status = (int)InvoiceStatus.Normal,
                    PaidAmount = 0,
                    Tip = 0,
                    InvoiceNumber = "",
                    IsTakeAway = IsTakeAway == true ? 1 : 0,
                    FkTable = CurrentBillBindProp.FkTable,
                    CloseDate = DateTime.Now.ToString(),
                    CustomerCount = 1
                };

                Invoice inv;
                if (IsEditingBill)
                {
                    inv = await invoiceLogic.UpdateAsync(invoice, false);
                }
                else
                {
                    inv = await invoiceLogic.CreateAsync(invoice, false);
                    //await connection.SendAsync("SendInvoice", invoice);
                }

                await tableLogic.ChangeStatusAsync(new Table
                {
                    Id = CurrentBillBindProp.FkTable,
                    IsSelected = 1
                }, false);


                foreach (var item in CurrentBillBindProp.ListItemInBill)
                {
                    if (item.Status == Status.New)
                    {
                        await invoiceItemLogic.CreateAsync(new InvoiceItemOrDiscount
                        {
                            FkInvoice = invoice.Id,
                            FkItemOrDiscount = item.Id,
                            Quantity = item.Quantity,
                            IsDiscount = 0,
                            Value = item.Value
                        }, false);

                        item.Status = Status.Normal;

                        for (int i = 1; i < item.ListSubItem.Count; i++)
                        {
                            await subItemLogic.CreateAsync(new ItemDiscount
                            {
                                FkItem = item.Id,
                                FkDiscount = item.ListSubItem[i].Id,
                                Value = item.ListSubItem[i].Value
                            }, false);

                            item.ListSubItem[i].Status = Status.Normal;
                        }

                    }
                }


                foreach (var discount in CurrentBillBindProp.ListDiscount)
                {
                    if (discount.Status == Status.New)
                    {
                        await invoiceItemLogic.CreateAsync(new InvoiceItemOrDiscount
                        {
                            FkInvoice = invoice.Id,
                            FkItemOrDiscount = discount.Id,
                            Quantity = discount.Quantity,
                            IsDiscount = 1,
                            Value = discount.Value
                        }, false);

                        discount.Status = Status.Normal;
                    }
                }

                await _dbContext.SaveChangesAsync();

                CurrentBillBindProp.CreationDate = inv.CreationDate;
                if (!IsEditingBill)
                {
                    ListInvoiceBindProp.Add(new VisualInvoiceModel(CurrentBillBindProp));
                }

                IsVisibleFrameHoaDonKhuVucBindProp = true;
                IsVisibleFrameHoaDonBindProp = true;

                IsVisibleFrameThuVienBindProp = false;
                IsVisibleFrameThucDonBindProp = false;
                IsVisibleFrameBillBindProp = false;

                //reset bill
                CurrentBillBindProp = new VisualInvoiceModel();
                _oldTable = null;
                IsEditingBill = false;
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
            SaveInvoiceCommand = new DelegateCommand<object>(OnSaveInvoice, CanExecuteSaveInvoice);
            SaveInvoiceCommand.ObservesProperty(() => IsNotBusy);
            SaveInvoiceCommand.ObservesProperty(() => CurrentBillBindProp.ListItemInBill);
        }

        #endregion

        #region InstantPayCommand

        public DelegateCommand<object> InstantPayCommand { get; private set; }
        private bool CanExecuteInstantPay(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (CurrentBillBindProp.ListItemInBill.Count == 0)
            {
                return false;
            }
            return true;
        }
        private async void OnInstantPay(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day

                // Tao param de pass data sang trang khac
                if (string.IsNullOrWhiteSpace(CurrentBillBindProp.FkTable))
                {
                    if (CurrentBillBindProp.IsTakeAway == 0)
                    {
                        await PageDialogService.DisplayAlertAsync("", "Bạn chưa chọn bàn!", "OK");
                        return;
                    }
                }
                var param = new NavigationParameters();
                param.Add(Keys.BILL, CurrentBillBindProp);
                if (IsEditingBill)
                {
                    param.Add(Keys.IS_EDITING, true);
                }
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
        private void InitInstantPayCommand()
        {
            InstantPayCommand = new DelegateCommand<object>(OnInstantPay, CanExecuteInstantPay);
            InstantPayCommand.ObservesProperty(() => IsNotBusy);
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

                var listItem = new List<VisualItemMenuModel>();
                _itemInMenu.TryGetValue(menu.Id, out listItem);
                ListItemInMenuBindProp = new ObservableCollection<VisualItemMenuModel>(listItem);
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
                await NavigationService.NavigateAsync(nameof(CSM_12Page), param);
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
                        case "billmode":
                            IsVisibleBillModeBindProp = true;
                            break;
                        case "inplace":
                            IsTakeAway = false;
                            IsVisibleBillModeBindProp = false;
                            break;
                        case "togo":
                            IsTakeAway = true;
                            IsVisibleBillModeBindProp = false;
                            break;
                        case "hamburger":
                            IsVisiblePopupBindProp = true;
                            break;
                        case "hoadon":
                            if (CurrentBillBindProp.ListDiscount.Count > 0 || CurrentBillBindProp.ListItemInBill.Count > 0)
                            {
                                var canNavigate = await PageDialogService.DisplayAlertAsync("Cảnh báo!", "Hóa đơn sẽ bị hủy, bạn có muốn tiếp tục?", "Đồng ý", "Hủy");
                                if (canNavigate)
                                {
                                    CurrentBillBindProp = new VisualInvoiceModel();
                                    IsEditingBill = false;
                                }
                                else
                                {
                                    break;
                                }
                            }
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
                        case "back":
                            IsVisibleFrameItemBindProp = false;
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
                    CurrentBillBindProp.TableName = $"{zone.ZoneName} - {table.TableName}";
                    CurrentBillBindProp.FkTable = table.Id;
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

        #region SelectInvoiceCommand

        public DelegateCommand<object> SelectInvoiceCommand { get; private set; }
        private async void OnSelectInvoice(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var bill = obj as VisualInvoiceModel;
                CurrentBillBindProp = bill;

                foreach (var zone in _tableInZone)
                {
                    var listTable = zone.Value;
                    if (listTable.Any(h => h.Id == CurrentBillBindProp.FkTable))
                    {
                        _oldTable = listTable.FirstOrDefault(h => h.Id == CurrentBillBindProp.FkTable);
                    }
                }
                IsEditingBill = true;
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
                OnSelectView("thuvien");
            }

        }
        [Initialize]
        private void InitSelectInvoiceCommand()
        {
            SelectInvoiceCommand = new DelegateCommand<object>(OnSelectInvoice);
            SelectInvoiceCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region PayCommand

        public DelegateCommand<object> PayCommand { get; private set; }
        private async void OnPay(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var bill = obj as VisualInvoiceModel;

                var param = new NavigationParameters();
                param.Add(Keys.BILL, bill);
                param.Add(Keys.IS_EDITING, true);
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
        private void InitPayCommand()
        {
            PayCommand = new DelegateCommand<object>(OnPay);
            PayCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteCommand

        public DelegateCommand<object> DeleteCommand { get; private set; }
        private async void OnDelete(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (IsNotDeleting)
                {
                    IsDeleting = true;
                }
                else
                {
                    var bill = obj as VisualInvoiceModel;
                    if (bill != null)
                    {
                        var invoiceLogic = new InvoiceLogic(_dbContext);
                        await invoiceLogic.DeleteAsync(bill.Id);
                        ListInvoiceBindProp.Remove(bill);
                    }
                    else
                    {
                        IsDeleting = false;
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
        private void InitDeleteCommand()
        {
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            DeleteCommand.ObservesCanExecute(() => IsNotBusy);
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
                param.Add(Keys.LIST_ITEM, ListItem);
                param.Add(Keys.MENU, _menuId);
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

        #region SelectItemInMenuCommand

        public DelegateCommand<object> SelectItemInMenuCommand { get; private set; }
        private async void OnSelectItemInMenu(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is VisualItemMenuModel item)
                {
                    if (IsNotEditting)
                    {
                        if (item.IsDiscount)
                        {
                            if (CurrentBillBindProp.ListDiscount.Any(h => h.Id == item.Id))
                            {
                                return;
                            }
                            if (item.IsInPercent)
                            {
                                var discount = new VisualItemMenuModel
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Value = item.Value * CurrentBillBindProp.TotalPrice / 100,
                                    IsInPercent = item.IsInPercent,
                                    Status = Status.New,
                                    //Giu gia tri % lai
                                    Quantity = (int)item.Value
                                };
                                CurrentBillBindProp.ListDiscount.Add(discount);
                                CurrentBillBindProp.TotalPrice -= discount.Value;
                            }
                            else
                            {
                                var discount = new VisualItemMenuModel
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Value = item.Value,
                                    IsInPercent = item.IsInPercent,
                                    Status = Status.New,
                                    Quantity = 1
                                };
                                CurrentBillBindProp.ListDiscount.Add(discount);
                                CurrentBillBindProp.TotalPrice -= discount.Value;
                            }
                            if (CurrentBillBindProp.TotalPrice < 0)
                            {
                                CurrentBillBindProp.TotalPrice = 0;
                            }
                        }
                        else
                        {
                            IsVisibleFrameItemBindProp = true;
                            SelectedItem = new VisualItemMenuModel
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Value = item.Value,
                                Quantity = 0
                            };
                        }
                    }
                    else
                    {
                        var canDelete = await DisplayDeleteAlertAsync();
                        if (canDelete)
                        {
                            var menuItemLogic = new MenuItemLogic(_dbContext);
                            await menuItemLogic.DeleteAsync(_menuId, item.Id);
                            ListItemInMenuBindProp.Remove(item);
                        }
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
        private void InitSelectItemInMenuCommand()
        {
            SelectItemInMenuCommand = new DelegateCommand<object>(OnSelectItemInMenu);
            SelectItemInMenuCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        // Frame mat hang

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }

        private async void OnSave(object obj)
        {
            if (IsBusy)
            {
                return;
            }
            if (SelectedItem.Quantity == 0)
            {
                await PageDialogService.DisplayAlertAsync("", "Bạn chưa chọn số lượng!", "OK");
                return;
            }
            IsBusy = true;
            try
            {
                // Thuc hien cong viec tai day
                //Them don gia
                SelectedItem.ListSubItem.Add(new VisualItemMenuModel
                {
                    Name = "Đơn giá",
                    Value = SelectedItem.Value
                });
                //Tinh tong tien
                SelectedItem.Value = SelectedItem.Value * SelectedItem.Quantity;
                SelectedItem.Status = Status.New;
                //Them giam gia
                foreach (var discount in ListDiscountBindProp)
                {
                    if (discount.IsSelected)
                    {
                        //Tinh tien sau giam gia
                        if (discount.IsInPercent)
                        {
                            var newDiscount = new VisualItemMenuModel
                            {
                                Id = discount.Id,
                                Name = discount.Name,
                                IsInPercent = discount.IsInPercent,
                                Status = Status.New,
                                Value = discount.Value * SelectedItem.Value / 100
                            };
                            SelectedItem.ListSubItem.Add(newDiscount);
                            SelectedItem.Value -= newDiscount.Value;
                        }
                        else
                        {
                            var newDiscount = new VisualItemMenuModel
                            {
                                Id = discount.Id,
                                Name = discount.Name,
                                IsInPercent = discount.IsInPercent,
                                Status = Status.New,
                                Value = discount.Value
                            };
                            SelectedItem.ListSubItem.Add(newDiscount);
                            SelectedItem.Value -= newDiscount.Value;
                        }

                        discount.IsSelected = false;
                    }
                }
                CurrentBillBindProp.ListItemInBill.Add(SelectedItem);
                CurrentBillBindProp.OriginalPrice += SelectedItem.Value;
                CurrentBillBindProp.TotalPrice = CurrentBillBindProp.OriginalPrice;
                foreach (var discount in CurrentBillBindProp.ListDiscount)
                {
                    if (discount.IsInPercent)
                    {
                        discount.Value = discount.Quantity * CurrentBillBindProp.OriginalPrice / 100;
                    }
                    CurrentBillBindProp.TotalPrice -= discount.Value;
                }
                CurrentBillBindProp.ItemCount += SelectedItem.Quantity;
                SelectedItem = null;
                IsVisibleFrameItemBindProp = false;
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
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectDiscountCommand

        public DelegateCommand<object> SelectDiscountCommand { get; private set; }
        private async void OnSelectDiscount(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var discount = obj as VisualItemMenuModel;
                if (discount.IsSelected)
                {
                    discount.IsSelected = false;
                }
                else
                {
                    discount.IsSelected = true;
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
        private void InitSelectDiscountCommand()
        {
            SelectDiscountCommand = new DelegateCommand<object>(OnSelectDiscount);
            SelectDiscountCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion

        #region Method
        private async void SaveInvoice(Invoice invoice)
        {
            var invoiceLogic = new InvoiceLogic(_dbContext);
            await invoiceLogic.CreateAsync(invoice);
        }
        private async void GetAllInvoice()
        {
            var invoiceLogic = new InvoiceLogic(_dbContext);
            var invoiceItemLogic = new InvoiceItemLogic(_dbContext);
            var subItemLogic = new ItemDiscountLogic(_dbContext);
            var tableLogic = new TableLogic(_dbContext);
            var zoneLogic = new ZoneLogic(_dbContext);

            var listTable = await tableLogic.GetAllAsync();
            var listInvoice = await invoiceLogic.GetAllAsync(InvoiceStatus.Normal);
            var listVisualInvoice = Mapper.Map<List<VisualInvoiceModel>>(listInvoice);
            var listZone = await zoneLogic.GetAllAsync();
            foreach (var invoice in listVisualInvoice)
            {
                var listInvoiceItem = await invoiceItemLogic.GetAsync(invoice.Id);
                foreach (var invoiceItem in listInvoiceItem)
                {
                    if (invoiceItem.IsDiscount == 1)
                    {
                        var item = ListDiscountBindProp.First(h => h.Id == invoiceItem.FkItemOrDiscount);
                        var visualItem = new VisualItemMenuModel
                        {
                            Id = item.Id,
                            Quantity = invoiceItem.Quantity,
                            Name = item.Name,
                            Status = Status.Normal,
                            Value = invoiceItem.Value,
                        };
                        invoice.ListDiscount.Add(visualItem);
                    }
                    else
                    {
                        var item = ListItem.First(h => h.Id == invoiceItem.FkItemOrDiscount);
                        var listSubItem = await subItemLogic.GetAsync(item.Id);

                        var visualItem = new VisualItemMenuModel
                        {
                            Id = item.Id,
                            Quantity = invoiceItem.Quantity,
                            Name = item.Name,
                            Status = Status.Normal,
                            Value = invoiceItem.Value,
                        };
                        visualItem.ListSubItem.Add(new VisualItemMenuModel
                        {
                            Name = "Đơn giá",
                            Value = item.Value,
                        });

                        foreach (var subItem in listSubItem)
                        {
                            var visualSubItem = ListDiscountBindProp.FirstOrDefault(h => h.Id == subItem.FkDiscount);
                            visualItem.ListSubItem.Add(new VisualItemMenuModel
                            {
                                Id = visualSubItem.Id,
                                Name = visualSubItem.Name,
                                Value = subItem.Value,
                                Status = Status.Normal,
                            });
                        }

                        invoice.ListItemInBill.Add(visualItem);
                        invoice.ItemCount += invoiceItem.Quantity;
                        invoice.OriginalPrice += invoiceItem.Value;
                    }
                    var table = listTable.First(h => h.Id == invoice.FkTable);
                    var zone = listZone.First(h => h.Id == table.FkZone);
                    invoice.TableName = $"{zone.ZoneName} - {table.TableName}";
                }
            }
            ListInvoiceBindProp = new ObservableCollection<VisualInvoiceModel>(listVisualInvoice);
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

            ListItem = new List<VisualItemMenuModel>(listVisualItem);
            ListDiscountBindProp = new ObservableCollection<VisualItemMenuModel>(listVisualDiscount);
            ListItemBindProp = new ObservableCollection<VisualItemMenuModel>(listVisualItem);

            var listVisualCategory = Mapper.Map<List<VisualCategoryModel>>(listCategory);
            ListCategoryBindProp = new ObservableCollection<VisualCategoryModel>(listVisualCategory);

            // lay danh sach menu va item trong tung menu
            var menuLogic = new MenuLogic(_dbContext);
            var menuItemLogic = new MenuItemLogic(_dbContext);

            var listMenu = await menuLogic.GetAllAsync();
            var listVisualMenu = Mapper.Map<List<VisualMenuModel>>(listMenu);

            _itemInMenu = new Dictionary<string, List<VisualItemMenuModel>>();
            //Lay danh sach item trong tung menu
            foreach (var menu in listMenu)
            {
                var listItemIdInMenu = await menuItemLogic.GetAsync(menu.Id);
                List<VisualItemMenuModel> listItemInMenu = new List<VisualItemMenuModel>();
                //lay thong tin tung item
                foreach (var item in listItemIdInMenu)
                {
                    listItemInMenu.Add(listVisualItem.FirstOrDefault(h => h.Id == item.FkItem));
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
                _itemInMenu.Add(visualMenu.Id, new List<VisualItemMenuModel>());
            }
            ListMenuBindProp = new ObservableCollection<VisualMenuModel>(listVisualMenu);

        }

        #endregion

        #region MessageHandler
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
        private void ReceiveCategoryMessageHandler(VisualCategoryModel category)
        {
            switch (category.Status)
            {
                case Status.New:
                    ListCategoryBindProp.Add(category);
                    break;
                case Status.Normal:
                    break;
                case Status.Modified:
                    var cate = ListCategoryBindProp.FirstOrDefault(h => h.Id == category.Id);
                    cate.CategoryName = category.CategoryName;
                    break;
                case Status.Deleted:
                    var categ = ListCategoryBindProp.FirstOrDefault(h => h.Id == category.Id);
                    ListCategoryBindProp.Remove(categ);
                    break;
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
                    //Back ve tu CSM 02
                    if (parameters.ContainsKey(Keys.ITEM))
                    {
                        var item = parameters[Keys.ITEM] as VisualItemMenuModel;
                        if (item.Status == Status.Deleted)
                        {
                            var deletedItem = ListItem.Find(h => h.Id == item.Id);
                            ListItem.Remove(deletedItem);
                        }
                        else
                        {
                            ListItem.Add(item);
                        }
                    }
                    //Back ve tu CSM 11
                    if (parameters.ContainsKey(Keys.LIST_ITEM))
                    {
                        var listItems = parameters[Keys.LIST_ITEM] as List<VisualItemMenuModel>;

                        foreach (var item in listItems)
                        {
                            if (!ListItemInMenuBindProp.Any(h => h.Id == item.Id))
                            {
                                ListItemInMenuBindProp.Add(item);
                                _itemInMenu[_menuId].Add(item);
                            }
                        }
                    }
                    //Back ve tu CSM 012
                    if (parameters.ContainsKey(Keys.MENU))
                    {
                        var menu = parameters[Keys.MENU] as VisualMenuModel;
                        if (menu.IsDeleted == 1)
                        {
                            ListMenuBindProp.Remove(menu);
                        }
                    }
                    //Back ve tu CSM 03
                    if (parameters.ContainsKey(Keys.DISCOUNT))
                    {
                        var discount = parameters[Keys.DISCOUNT] as VisualItemMenuModel;
                        if (discount.Status == Status.Deleted)
                        {
                            var deletedItem = ListDiscountBindProp.First(h => h.Id == discount.Id);
                            ListDiscountBindProp.Remove(deletedItem);
                        }
                        else
                        {
                            ListDiscountBindProp.Add(discount);
                        }
                        IsVisibleListCategoryBindProp = true;
                    }
                    //Back ve tu CSM 10
                    if (parameters.ContainsKey(Keys.BILL))
                    {
                        CurrentBillBindProp = new VisualInvoiceModel();
                        var billId = parameters[Keys.BILL] as string;

                        var invoice = ListInvoiceBindProp.FirstOrDefault(h => h.Id == billId);
                        if (invoice != null)
                        {
                            ListInvoiceBindProp.Remove(invoice);
                        }
                    }
                    break;
                case NavigationMode.New:
                    //await connection.StartAsync();
                    Title = "Thư viện";
                    GetAllMenuItem();
                    GetAllZone();
                    GetAllInvoice();
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
