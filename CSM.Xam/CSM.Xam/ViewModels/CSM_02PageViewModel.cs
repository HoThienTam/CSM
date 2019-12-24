using CSM.EFCore;
using CSM.Logic;
using CSM.Logic.Enums;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.ViewModels
{
    public class CSM_02PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_02PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

        }

        #region Bind Property

        #region CategoryBindProp
        private VisualCategoryModel _CategoryBindProp = null;
        public VisualCategoryModel CategoryBindProp
        {
            get { return _CategoryBindProp; }
            set { SetProperty(ref _CategoryBindProp, value); }
        }
        #endregion

        #region ItemBindProp
        private VisualItemMenuModel _ItemBindProp = new VisualItemMenuModel();
        public VisualItemMenuModel ItemBindProp
        {
            get { return _ItemBindProp; }
            set { SetProperty(ref _ItemBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
        }
        #endregion

        #region IsManaged
        private bool _IsManaged = false;
        public bool IsManaged
        {
            get { return _IsManaged; }
            set { SetProperty(ref _IsManaged, value); }
        }
        #endregion

        #region MinQuantityBindProp
        private long _MinQuantityBindProp = 0;
        public long MinQuantityBindProp
        {
            get { return _MinQuantityBindProp; }
            set { SetProperty(ref _MinQuantityBindProp, value); }
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
                await NavigationService.NavigateAsync(nameof(CSM_02_01Page));
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
            if (string.IsNullOrWhiteSpace(ItemBindProp.Name))
            {
                return false;
            }
            if (ItemBindProp.Value <= 0)
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
                var itemLogic = new ItemLogic(_dbContext);
                var item = new Item
                {
                    Id = ItemBindProp.Id,
                    ItemName = ItemBindProp.Name,
                    FkCategory = CategoryBindProp.Id,
                    Price = ItemBindProp.Value,
                    IsManaged = IsManaged == true ? 1 : 0,
                    MinQuantity = MinQuantityBindProp,
                };
                ItemBindProp.FkCategory = CategoryBindProp.Id;
                if (IsEditing)
                {
                    await itemLogic.UpdateAsync(item);
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    await itemLogic.CreateAsync(item);

                    var param = new NavigationParameters();                   
                    param.Add(Keys.ITEM, ItemBindProp);

                    await NavigationService.GoBackAsync(param);
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
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave, CanExecuteSave);
            SaveCommand.ObservesProperty(() => IsNotBusy);
            SaveCommand.ObservesProperty(() => ItemBindProp.Name);
            SaveCommand.ObservesProperty(() => ItemBindProp.Value);
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
                var canDelete = await DisplayDeleteAlertAsync();
                if (canDelete)
                {
                    var itemLogic = new ItemLogic(_dbContext);
                    await itemLogic.DeleteAsync(ItemBindProp.Id);
                    ItemBindProp.Status = Status.Deleted;
                    var param = new NavigationParameters();
                    param.Add(Keys.ITEM, ItemBindProp);
                    await NavigationService.GoBackAsync(param);
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

        #endregion

        #region Navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey(Keys.CATEGORY))
                    {
                        CategoryBindProp = parameters[Keys.CATEGORY] as VisualCategoryModel;
                    }
                    break;
                case NavigationMode.New:
                    if (parameters.ContainsKey(Keys.ITEM))
                    {
                        IsEditing = true;
                        Title = "Chỉnh sửa mặt hàng";
                        var item = parameters[Keys.ITEM] as VisualItemMenuModel;
                        var category = parameters[Keys.CATEGORY] as VisualCategoryModel;

                        var itemLogic = new ItemLogic(_dbContext);
                        var dbItem = await itemLogic.GetAsync(item.Id);

                        MinQuantityBindProp = dbItem.MinQuantity;
                        IsManaged = dbItem.IsManaged == 1 ? true : false;
                        ItemBindProp = item;
                        CategoryBindProp = category;
                    }
                    else
                    {
                        IsEditing = false;
                        ItemBindProp.Id = Guid.NewGuid().ToString();
                        Title = "Tạo mặt hàng";
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
