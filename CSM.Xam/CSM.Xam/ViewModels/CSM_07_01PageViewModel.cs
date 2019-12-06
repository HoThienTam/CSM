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
                RaisePropertyChanged(nameof(IsNotEditing));
            }
        }
        public bool IsNotEditing { get { return !_IsEditing; } }
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
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PasswordBindProp))
            {
                ErrorBindProp = "Chưa nhập mật khẩu!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(ConfirmedPasswordBindProp))
            {
                ErrorBindProp = "Chưa nhập lại mật khẩu!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmployeeBindProp.EmployeeName))
            {
                ErrorBindProp = "Chưa nhập tên tài khoản!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmployeeBindProp.FullName))
            {
                ErrorBindProp = "Chưa nhập tên nhân viên!";
                return false;
            }
            if (PasswordBindProp != ConfirmedPasswordBindProp)
            {
                ErrorBindProp = "Mật khẩu không trùng khớp!";
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
                var employeeLogic = new EmployeeLogic(_dbContext);
                if (IsEditing)
                {

                }
                else
                {
                    
                    await employeeLogic.CreateAsync(new Employee
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeName = EmployeeBindProp.EmployeeName,
                        FullName = EmployeeBindProp.FullName,
                        Password = PasswordBindProp,
                        Role = 1
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
            SaveCommand = new DelegateCommand<object>(OnSave, CanExecuteSave);
            SaveCommand.ObservesProperty(() => IsNotBusy);
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
                        Title = "Đổi mật khẩu";
                    }                  
                    else
                    {
                        IsEditing = false;
                        Title = "Thêm nhân viên mới";
                        EmployeeBindProp = new VisualEmployeeModel();
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
