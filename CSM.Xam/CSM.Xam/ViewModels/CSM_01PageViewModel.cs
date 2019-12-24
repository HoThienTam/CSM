using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CSM.Xam.ViewModels
{
    public class CSM_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {         

        }

        #region UsernameBindProp
        private string _UsernameBindProp = string.Empty;
        public string UsernameBindProp
        {
            get { return _UsernameBindProp; }
            set { SetProperty(ref _UsernameBindProp, value); }
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

        #region ErrorBindProp
        private string _ErrorBindProp = string.Empty;
        public string ErrorBindProp
        {
            get { return _ErrorBindProp; }
            set { SetProperty(ref _ErrorBindProp, value); }
        }
        #endregion

        #region LoginCommand

        public DelegateCommand<object> LoginCommand { get; private set; }
        private bool CanExecuteLogin(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(UsernameBindProp))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PasswordBindProp))
            {
                return false;
            }
            return true;
        }
        private async void OnLogin(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var employeeLogic = new EmployeeLogic(_dbContext);

                var canLogin = await employeeLogic.LoginAsync(UsernameBindProp, PasswordBindProp);
                if (canLogin)
                {
                    Application.Current.Properties["Employee"] = "Tam";
                    await Application.Current.SavePropertiesAsync();

                    await NavigationService.NavigateAsync(nameof(MainPage));
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Lỗi", "Đăng nhập thất bại, vui lòng kiểm tra lại tên đăng nhập hoặc mật khẩu.", "Đóng");
                    PasswordBindProp = "";
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
        private void InitLoginCommand()
        {
            LoginCommand = new DelegateCommand<object>(OnLogin, CanExecuteLogin);
            LoginCommand.ObservesProperty(() => IsNotBusy);
            LoginCommand.ObservesProperty(() => UsernameBindProp);
            LoginCommand.ObservesProperty(() => PasswordBindProp);
        }

        #endregion

    }
}
