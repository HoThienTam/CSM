using CSM.EFCore;
using CSM.Logic;
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

        #region ItemNameBindProp
        private string _ItemNameBindProp = string.Empty;
        public string ItemNameBindProp
        {
            get { return _ItemNameBindProp; }
            set { SetProperty(ref _ItemNameBindProp, value); }
        }
        #endregion

        #region PriceBindProp
        private double _PriceBindProp = 0;
        public double PriceBindProp
        {
            get { return _PriceBindProp; }
            set { SetProperty(ref _PriceBindProp, value); }
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
            if (string.IsNullOrWhiteSpace(ItemNameBindProp))
            {
                return false;
            }
            if (PriceBindProp <= 0)
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
                    Id = Guid.NewGuid().ToString(),
                    ItemName = ItemNameBindProp,
                    FkCategory = CategoryBindProp.Id,
                    Price = PriceBindProp
                };
                await itemLogic.CreateAsync(item);

                var param = new NavigationParameters();
                param.Add(Keys.ITEM, item);

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
            SaveCommand.ObservesProperty(() => ItemNameBindProp);
            SaveCommand.ObservesProperty(() => PriceBindProp);
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
