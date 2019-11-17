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
using Telerik.XamarinForms.DataControls.ListView.Commands;

namespace CSM.Xam.ViewModels
{
    public class CSM_11PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        private string _menuId;
        private List<Item> _listAllItem;
        public CSM_11PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Thêm vào thực đơn";
        }

        #region Bind Prop

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
        private List<Item> _SelectedItems;
        public List<Item> SelectedItems
        {
            get { return _SelectedItems; }
            set { SetProperty(ref _SelectedItems, value); }
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
                var selectedCategory = (obj as ItemTapCommandContext).Item as Category;

                var listItem = _listAllItem.Where(h => h.FkCategory == selectedCategory.Id).ToList();
                // Chuyen item thanh item extended
                ListItemBindProp = new ObservableCollection<ItemExtended>();
                foreach (var item in listItem)
                {
                    ListItemBindProp.Add(new ItemExtended 
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        IsSelected = false,
                        Price = item.Price
                    });
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

                var menuItemLogic = new MenuItemLogic(_dbContext);

                //luu vao database
                await menuItemLogic.CreateAsync(_menuId, SelectedItems);

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
                    SelectedItems = new List<Item>();
                }

                var selectedItem = (obj as ItemTapCommandContext).Item as ItemExtended;
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
                    ListCategoryBindProp = parameters[Keys.LIST_CATEGORY] as ObservableCollection<Category>;
                    _listAllItem = parameters[Keys.LIST_ITEM] as List<Item>;
                    _menuId = parameters[Keys.ZONE] as string;
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
