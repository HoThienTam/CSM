using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.ViewModels
{
    class CSM_08_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_08_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

        }

        #region ZoneNameBindProp
        private string _ZoneNameBindProp = string.Empty;
        public string ZoneNameBindProp
        {
            get { return _ZoneNameBindProp; }
            set { SetProperty(ref _ZoneNameBindProp, value); }
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
            if (string.IsNullOrWhiteSpace(ZoneNameBindProp))
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
                var zoneLogic = new ZoneLogic(_dbContext);

                await zoneLogic.CreateAsync(new Zone
                {
                    Id = Guid.NewGuid().ToString(),
                    ZoneName = ZoneNameBindProp,
                });
                await NavigationService.GoBackAsync();
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
            SaveCommand.ObservesProperty(() => ZoneNameBindProp);
        }

        #endregion

    }
}
