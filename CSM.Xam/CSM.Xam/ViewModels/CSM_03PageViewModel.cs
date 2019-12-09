using System;
using System.Collections.Generic;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using Prism.Navigation;
using Telerik.XamarinForms.Primitives.CheckBox.Commands;

namespace CSM.Xam.ViewModels
{
    public class CSM_03PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_03PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Tạo giảm giá";
        }
        #region Bindprop

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

        #region DiscountBindProp
        private VisualItemMenuModel _DiscountBindProp = new VisualItemMenuModel();
        public VisualItemMenuModel DiscountBindProp
        {
            get { return _DiscountBindProp; }
            set { SetProperty(ref _DiscountBindProp, value); }
        }
        #endregion

        #region IsPercentCheckedBindProp
        private bool _IsPercentCheckedBindProp = true;
        public bool IsPercentCheckedBindProp
        {
            get { return _IsPercentCheckedBindProp; }
            set
            {
                SetProperty(ref _IsPercentCheckedBindProp, value);
                RaisePropertyChanged(nameof(IsMoneyCheckedBindProp));
            }
        }
        public bool IsMoneyCheckedBindProp
        {
            get { return !_IsPercentCheckedBindProp; }
        }
        #endregion

        #region CategoryDiscountValueBindProp
        private double _CategoryDiscountValueBindProp = 0;
        public double CategoryDiscountValueBindProp
        {
            get { return _CategoryDiscountValueBindProp; }
            set { SetProperty(ref _CategoryDiscountValueBindProp, value); }
        }
        #endregion

        #region NormalDiscountValueBindProp
        private double _NormalDiscountValueBindProp = 0;
        public double NormalDiscountValueBindProp
        {
            get { return _NormalDiscountValueBindProp; }
            set { SetProperty(ref _NormalDiscountValueBindProp, value); }
        }
        #endregion

        #region NormalDiscountPercentBindProp
        private double _NormalDiscountPercentBindProp = 0;
        public double NormalDiscountPercentBindProp
        {
            get { return _NormalDiscountPercentBindProp; }
            set { SetProperty(ref _NormalDiscountPercentBindProp, value); }
        }
        #endregion

        #region CategoryBindProp
        private VisualCategoryModel _CategoryBindProp = new VisualCategoryModel();
        public VisualCategoryModel CategoryBindProp
        {
            get { return _CategoryBindProp; }
            set { SetProperty(ref _CategoryBindProp, value); }
        }
        #endregion

        #endregion

        #region Commands

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
                    var discountLogic = new DiscountLogic(_dbContext);
                    await discountLogic.DeleteAsync(DiscountBindProp.Id);

                    DiscountBindProp.IsDeleted = true;
                    var param = new NavigationParameters();
                    param.Add(Keys.DISCOUNT, DiscountBindProp);
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

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(DiscountBindProp.Name))
            {
                return false;
            }
            if (NormalDiscountPercentBindProp == 0 && NormalDiscountValueBindProp == 0)
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
                if (IsEditing)
                {
                    if (IsMoneyCheckedBindProp)
                    {
                        await discountLogic.UpdateAsync(new Discount
                        {
                            Id = DiscountBindProp.Id,
                            DiscountName = DiscountBindProp.Name,
                            DiscountValue = NormalDiscountValueBindProp,
                            IsInPercent = 0,
                            MaxValue = 0,
                        });
                        DiscountBindProp.Value = NormalDiscountValueBindProp;
                        DiscountBindProp.IsInPercent = false;
                    }
                    else
                    {
                        await discountLogic.UpdateAsync(new Discount
                        {
                            Id = DiscountBindProp.Id,
                            DiscountName = DiscountBindProp.Name,
                            DiscountValue = NormalDiscountPercentBindProp,
                            IsInPercent = 1,
                            MaxValue = 0,
                        });
                        DiscountBindProp.Value = NormalDiscountPercentBindProp;
                        DiscountBindProp.IsInPercent = true;
                    }

                    await NavigationService.GoBackAsync();
                }
                else
                {
                    if (IsMoneyCheckedBindProp)
                    {
                        await discountLogic.CreateAsync(new Discount
                        {
                            Id = DiscountBindProp.Id,
                            DiscountName = DiscountBindProp.Name,
                            DiscountValue = NormalDiscountValueBindProp,
                            IsInPercent = 0,
                            MaxValue = 0,
                        });
                        DiscountBindProp.Value = NormalDiscountValueBindProp;
                        DiscountBindProp.IsInPercent = false;
                    }
                    else
                    {
                        await discountLogic.CreateAsync(new Discount
                        {
                            Id = DiscountBindProp.Id,
                            DiscountName = DiscountBindProp.Name,
                            DiscountValue = NormalDiscountPercentBindProp,
                            IsInPercent = 1,
                            MaxValue = 0,
                        });
                        DiscountBindProp.Value = NormalDiscountPercentBindProp;
                        DiscountBindProp.IsInPercent = true;
                    }

                    var param = new NavigationParameters();
                    param.Add(Keys.DISCOUNT, DiscountBindProp);

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
        }

        #endregion

        #region IsCheckedChangedCommand

        public DelegateCommand<object> IsCheckedChangedCommand { get; private set; }
        private bool CanExecuteIsCheckedChanged(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (IsMoneyCheckedBindProp)
            {
                return false;
            }
            return true;
        }
        private async void OnIsCheckedChanged(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var isCheckedChanged = obj as CheckBoxIsCheckChangedCommandContext;
                if (isCheckedChanged.OldState == true)
                {
                    return;
                }
                IsPercentCheckedBindProp = false;
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
        private void InitIsCheckedChangedCommand()
        {
            IsCheckedChangedCommand = new DelegateCommand<object>(OnIsCheckedChanged, CanExecuteIsCheckedChanged);
            IsCheckedChangedCommand.ObservesProperty(() => IsNotBusy);
        }

        #endregion

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    //Back tu trang 02_01
                    if (parameters.ContainsKey(Keys.CATEGORY))
                    {
                        CategoryBindProp = parameters[Keys.CATEGORY] as VisualCategoryModel;
                    }
                    break;
                case NavigationMode.New:
                    if (parameters.ContainsKey(Keys.DISCOUNT))
                    {
                        IsEditing = true;
                        Title = "Chỉnh sửa giảm giá";
                        DiscountBindProp = parameters[Keys.DISCOUNT] as VisualItemMenuModel;
                        if (DiscountBindProp.IsInPercent)
                        {
                            NormalDiscountPercentBindProp = DiscountBindProp.Value;
                        }
                        else
                        {
                            NormalDiscountValueBindProp = DiscountBindProp.Value;
                        }
                    }
                    else
                    {
                        DiscountBindProp.Id = Guid.NewGuid().ToString();
                        DiscountBindProp.IsDiscount = true;
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
    }
}
