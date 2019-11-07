using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.ViewModels
{
    public class CSM_02PageViewModel : ViewModelBase
    {
        public CSM_02PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

        }

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

    }
}
