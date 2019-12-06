using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using Prism.Navigation;

namespace CSM.Xam.ViewModels
{
    public class CSM_07PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_07PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
        }

        #region ListEmployeeBindProp
        private ObservableCollection<VisualEmployeeModel> _ListEmployeeBindProp = null;
        public ObservableCollection<VisualEmployeeModel> ListEmployeeBindProp
        {
            get { return _ListEmployeeBindProp; }
            set { SetProperty(ref _ListEmployeeBindProp, value); }
        }
        #endregion

        #region SelectedEmployeeBindProp
        private VisualEmployeeModel _SelectedEmployeeBindProp = null;
        public VisualEmployeeModel SelectedEmployeeBindProp
        {
            get { return _SelectedEmployeeBindProp; }
            set { SetProperty(ref _SelectedEmployeeBindProp, value); }
        }
        #endregion

        #region AddEmployeeCommand

        public DelegateCommand<object> AddEmployeeCommand { get; private set; }
        private async void OnAddEmployee(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                await NavigationService.NavigateAsync(nameof(CSM_07_01Page));
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
        private void InitAddEmployeeCommand()
        {
            AddEmployeeCommand = new DelegateCommand<object>(OnAddEmployee);
            AddEmployeeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region ChangePasswordCommand

        public DelegateCommand<object> ChangePasswordCommand { get; private set; }
        private async void OnChangePassword(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var param = new NavigationParameters();
                param.Add(Keys.EMPLOYEE, SelectedEmployeeBindProp);

                await NavigationService.NavigateAsync(nameof(CSM_07_01Page), param);
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
        private void InitChangePasswordCommand()
        {
            ChangePasswordCommand = new DelegateCommand<object>(OnChangePassword);
            ChangePasswordCommand.ObservesCanExecute(() => IsNotBusy);
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
                    var employeeLogic = new EmployeeLogic(_dbContext);
                    await employeeLogic.DeleteAsync(SelectedEmployeeBindProp.Id);

                    var deletedEmployee = ListEmployeeBindProp.FirstOrDefault(h => h.Id == SelectedEmployeeBindProp.Id);
                    ListEmployeeBindProp.Remove(deletedEmployee);
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

        #region TapEmployeeCommand

        public DelegateCommand<object> TapEmployeeCommand { get; private set; }
        private async void OnTapEmployee(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                SelectedEmployeeBindProp.IsSelected = false;

                var employee = obj as VisualEmployeeModel;
                SelectedEmployeeBindProp = employee;
                SelectedEmployeeBindProp.IsSelected = true;
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
        private void InitTapEmployeeCommand()
        {
            TapEmployeeCommand = new DelegateCommand<object>(OnTapEmployee);
            TapEmployeeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        private async void GetEmployees()
        {
            var employeeLogic = new EmployeeLogic(_dbContext);
            var listEmployee = await employeeLogic.GetAllAsync();
            var listVisualEmployee = Mapper.Map<List<VisualEmployeeModel>>(listEmployee);
            ListEmployeeBindProp = new ObservableCollection<VisualEmployeeModel>(listVisualEmployee);
            SelectedEmployeeBindProp = ListEmployeeBindProp[0];
            SelectedEmployeeBindProp.IsSelected = true;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey(Keys.EMPLOYEE))
                    {
                        var employee = parameters[Keys.EMPLOYEE] as VisualEmployeeModel;
                        ListEmployeeBindProp.Add(employee);
                    }
                    break;
                case NavigationMode.New:
                    GetEmployees();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
            }
        }
    }
}
