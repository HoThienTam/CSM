using System;
using System.Collections.Generic;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;

namespace CSM.Xam.ViewModels
{
    public class CSM_07_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_07_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
        }

        #region BindProp

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set 
            { 
                SetProperty(ref _IsEditing, value);
            }
        }
        #endregion

        #region IsChangingPassword
        private bool _IsChangingPassword = false;
        public bool IsChangingPassword
        {
            get { return _IsChangingPassword; }
            set { SetProperty(ref _IsChangingPassword, value); }
        }
        #endregion

        #region IsChangingInfo
        private bool _IsChangingInfo = false;
        public bool IsChangingInfo
        {
            get { return _IsChangingInfo; }
            set { SetProperty(ref _IsChangingInfo, value); }
        }
        #endregion

        #region EmployeeBindProp
        private VisualEmployeeModel _EmployeeBindProp = null;
        public VisualEmployeeModel EmployeeBindProp
        {
            get { return _EmployeeBindProp; }
            set { SetProperty(ref _EmployeeBindProp, value); }
        }
        #endregion

        #region PasswordBindProp
        private string _PasswordBindProp = string.Empty;
        public string PasswordBindProp
        {
            get { return _PasswordBindProp; }
            set { SetProperty(ref _PasswordBindProp, value); }
        }
        #endregion

        #region ConfirmedPasswordBindProp
        private string _ConfirmedPasswordBindProp = string.Empty;
        public string ConfirmedPasswordBindProp
        {
            get { return _ConfirmedPasswordBindProp; }
            set { SetProperty(ref _ConfirmedPasswordBindProp, value); }
        }
        #endregion

        #region ErrorBindProp
        private string _ErrorBindProp = string.Empty;
        public string ErrorBindProp
        {
            get { return _ErrorBindProp; }
            set { SetProperty(ref _ErrorBindProp, value); }
        }
        #endregion

        #endregion

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private async void OnSave(object obj)
        {
            if (string.IsNullOrWhiteSpace(PasswordBindProp) && IsChangingPassword)
            {
                await PageDialogService.DisplayAlertAsync("", "Chưa nhập mật khẩu!", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmployeeBindProp.EmployeeName))
            {
                await PageDialogService.DisplayAlertAsync("", "Chưa nhập tên tài khoản!", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmployeeBindProp.FullName))
            {
                await PageDialogService.DisplayAlertAsync("", "Chưa nhập tên nhân viên!", "OK");
                return;
            }
            if (PasswordBindProp != ConfirmedPasswordBindProp)
            {
                await PageDialogService.DisplayAlertAsync("", "Mật khẩu không trùng khớp!", "OK");
                return;
            }
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var employeeLogic = new EmployeeLogic(_dbContext);
                if (IsEditing)
                {
                    if (IsChangingPassword)
                    {
                        await employeeLogic.UpdateAsync(new Employee
                        {
                            Id = EmployeeBindProp.Id,
                            Password = PasswordBindProp,
                        });
                    }
                    if (IsChangingInfo)
                    {
                        await employeeLogic.UpdateAsync(new Employee
                        {
                            Id = EmployeeBindProp.Id,
                            FullName = EmployeeBindProp.FullName,
                            Role = EmployeeBindProp.Role
                        });
                    }
                    await NavigationService.GoBackAsync();
                }
                else
                {                  
                    await employeeLogic.CreateAsync(new Employee
                    {
                        Id = EmployeeBindProp.Id,
                        EmployeeName = EmployeeBindProp.EmployeeName,
                        FullName = EmployeeBindProp.FullName,
                        Password = PasswordBindProp,
                        Role = EmployeeBindProp.Role
                    });

                    var param = new NavigationParameters();
                    param.Add(Keys.EMPLOYEE, EmployeeBindProp);

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
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
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
                if (EmployeeBindProp.Role == 0)
                {
                    EmployeeBindProp.Role = 1;
                }
                else
                {
                    EmployeeBindProp.Role = 0;
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
                    if (parameters.ContainsKey(Keys.EMPLOYEE))
                    {
                        IsEditing = true;
                        if (parameters.ContainsKey(Keys.IS_EDITING))
                        {
                            IsChangingInfo = true;
                            IsChangingPassword = false;
                            Title = "Chỉnh sửa nhân viên";
                            EmployeeBindProp = parameters[Keys.EMPLOYEE] as VisualEmployeeModel;
                        }
                        else
                        {
                            IsChangingPassword = true;
                            IsChangingInfo = false;
                            Title = "Đổi mật khẩu";
                            EmployeeBindProp = parameters[Keys.EMPLOYEE] as VisualEmployeeModel;
                        }
                    }                  
                    else
                    {
                        IsEditing = false;
                        IsChangingInfo = true;
                        IsChangingPassword = true;
                        Title = "Thêm nhân viên mới";
                        EmployeeBindProp = new VisualEmployeeModel();
                        EmployeeBindProp.Id = Guid.NewGuid().ToString();
                    }
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
            }
        }
    }
}
