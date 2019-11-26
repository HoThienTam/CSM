using System;
using System.Collections.Generic;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using Prism.Navigation;

namespace CSM.Xam.ViewModels
{
    public class CSM_03PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_03PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Tạo giảm giá";
        }

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set 
            { 
                SetProperty(ref _IsEditing, value);
                RaisePropertyChanged(nameof(IsNotEditing));
            }
        }
        public bool IsNotEditing { get { return !_IsEditing; } }
        #endregion

        #region IsNormalDiscount
        private bool _IsNormalDiscount = true;
        public bool IsNormalDiscount
        {
            get { return _IsNormalDiscount; }
            set 
            { 
                SetProperty(ref _IsNormalDiscount, value);
                RaisePropertyChanged(nameof(IsCategoryDiscount));
            }
        }
        public bool IsCategoryDiscount { get { return !_IsNormalDiscount; } }
    #endregion

        #region DiscountBindProp
        private VisualDiscountModel _DiscountBindProp = new VisualDiscountModel();
        public VisualDiscountModel DiscountBindProp
        {
            get { return _DiscountBindProp; }
            set { SetProperty(ref _DiscountBindProp, value); }
        }
        #endregion

        #region CategoryBindProp
        private CategoryExtended _CategoryBindProp = null;
        public CategoryExtended CategoryBindProp
        {
            get { return _CategoryBindProp; }
            set { SetProperty(ref _CategoryBindProp, value); }
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
                var accepted = await DisplayDeleteAlertAsync();
                if (accepted)
                {

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

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (DiscountBindProp.DiscountValue == 0 || string.IsNullOrWhiteSpace(DiscountBindProp.DiscountName))
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
                var discountLogic = new DiscountLogic(_dbContext);
                await discountLogic.CreateAsync(new Discount
                {
                    Id = Guid.NewGuid().ToString(),
                    DiscountName = DiscountBindProp.DiscountName,
                    DiscountValue = DiscountBindProp.DiscountValue,
                    IsInPercent = 0,
                    MaxValue = 0,
                    DiscountType = 0,
                });
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

        #region SelectedChangedCommand

        public DelegateCommand<object> SelectedChangedCommand { get; private set; }
        private async void OnSelectedChanged(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (IsNormalDiscount)
                {
                    IsNormalDiscount = false;
                }
                else
                {
                    IsNormalDiscount = true;
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
        private void InitSelectedChangedCommand()
        {
            SelectedChangedCommand = new DelegateCommand<object>(OnSelectedChanged);
            SelectedChangedCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
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
    }
}
