using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using Telerik.XamarinForms.DataControls.ListView;
using Telerik.XamarinForms.DataControls.ListView.Commands;

namespace CSM.Xam.ViewModels
{
    public class CSM_11PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        private string _menuId;
        private string _selectedCategory;
        public CSM_11PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Thêm vào thực đơn";
        }

        #region Bind Prop

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

        #region SelectedItems
        private List<VisualItemMenuModel> _SelectedItems;
        public List<VisualItemMenuModel> SelectedItems
        {
            get { return _SelectedItems; }
            set { SetProperty(ref _SelectedItems, value); }
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

        #endregion

        #region Command

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
                    _selectedCategory = (itemTap.Item as VisualCategoryModel).Id;
                    Title = (itemTap.Item as VisualCategoryModel).CategoryName;
                }
                if (obj is string category)
                {
                    switch (category)
                    {
                        case "discount":
                            _selectedCategory = "discount";
                            var listDiscount = ListItem.Where(h => h.IsDiscount == true).ToList();
                            ListItemBindProp = new ObservableCollection<VisualItemMenuModel>(listDiscount);
                            Title = "Giảm giá";
                            break;
                        case "allitem":
                            _selectedCategory = "allitem";
                            var listItem = ListItem.Where(h => h.IsDiscount == false).ToList();
                            ListItemBindProp = new ObservableCollection<VisualItemMenuModel>(listItem);
                            Title = "Tất cả mặt hàng";
                            break;
                    }
                }
                else
                {
                    var listItem = ListItem.Where(h => h.FkCategory == _selectedCategory).ToList();
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

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (SelectedItems == null || SelectedItems.Count == 0)
            {
                return false;
            }
            return true;
        }
        private async void OnSave(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var param = new NavigationParameters();
                param.Add(Keys.LIST_ITEM, SelectedItems);
                var listItem = new List<Item>();
                foreach (var item in SelectedItems)
                {
                    listItem.Add(new Item 
                    {
                        Id = item.Id,
                    });
                }
                var menuItemLogic = new MenuItemLogic(_dbContext);

                //luu vao database
                await menuItemLogic.CreateAsync(_menuId, listItem);

                await NavigationService.GoBackAsync(param);
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
            SaveCommand = new DelegateCommand<object>(OnSave, CanExecuteSave);
            SaveCommand.ObservesProperty(() => IsNotBusy);
            SaveCommand.ObservesProperty(() => SelectedItems);
        }

        #endregion

        #region TapItemCommand

        public DelegateCommand<object> TapItemCommand { get; private set; }
        private async void OnTapItem(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (SelectedItems == null)
                {
                    SelectedItems = new List<VisualItemMenuModel>();
                }

                var selectedItem = (obj as ItemTapCommandContext).Item as VisualItemMenuModel;
                if (selectedItem.IsSelected)
                {
                    selectedItem.IsSelected = false;
                    SelectedItems.Remove(selectedItem);
                }
                else
                {
                    selectedItem.IsSelected = true;
                    SelectedItems.Add(selectedItem);
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
        private void InitTapItemCommand()
        {
            TapItemCommand = new DelegateCommand<object>(OnTapItem);
            TapItemCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region GoBackCommand

        public new DelegateCommand<object> GoBackCommand { get; private set; }
        private async void OnGoBack(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (IsVisibleListItemBindProp)
                {
                    IsVisibleListCategoryBindProp = true;
                }
                else
                {
                    await NavigationService.GoBackAsync();
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
        private void InitGoBackCommand()
        {
            GoBackCommand = new DelegateCommand<object>(OnGoBack);
            GoBackCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion

        #region Navigate
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    ListCategoryBindProp = parameters[Keys.LIST_CATEGORY] as ObservableCollection<VisualCategoryModel>;
                    ListItem = parameters[Keys.LIST_ITEM] as List<VisualItemMenuModel>;
                    _menuId = parameters[Keys.MENU] as string;
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
