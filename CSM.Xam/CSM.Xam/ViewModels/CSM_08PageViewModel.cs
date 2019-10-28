using CSM.EFCore;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CSM.Xam.ViewModels
{
    public class CSM_08PageViewModel : ViewModelBase
    {
        public CSM_08PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            ListSectionBindProp = new ObservableCollection<Zone>();
            for (int i = 0; i < 8; i++)
            {
                ListSectionBindProp.Add(new Zone
                {
                    ZoneName = $"Khu vực {i}"
                }) ;
            }
        }

        #region ListSectionBindProp
        private ObservableCollection<Zone> _ListSectionBindProp = null;
        public ObservableCollection<Zone> ListSectionBindProp
        {
            get { return _ListSectionBindProp; }
            set { SetProperty(ref _ListSectionBindProp, value); }
        }
        #endregion

        #region TapSectionCommand

        public DelegateCommand<object> TapSectionCommand { get; private set; }
        private async void OnTapSection(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                await NavigationService.NavigateAsync(nameof(CSM_08_01Page));
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
        private void InitTapSectionCommand()
        {
            TapSectionCommand = new DelegateCommand<object>(OnTapSection);
            TapSectionCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
