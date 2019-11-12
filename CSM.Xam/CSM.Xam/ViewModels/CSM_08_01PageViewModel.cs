using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace CSM.Xam.ViewModels
{
    class CSM_08_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        private bool _isEditing = false;
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

        #region ZoneIdBindProp
        private string _ZoneIdBindProp = string.Empty;
        public string ZoneIdBindProp
        {
            get { return _ZoneIdBindProp; }
            set { SetProperty(ref _ZoneIdBindProp, value); }
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
                var zone = new Zone();
                //Neu chinh sua
                if (_isEditing)
                {
                    zone.Id = ZoneIdBindProp;
                    zone.ZoneName = ZoneNameBindProp;

                    await zoneLogic.UpdateAsync(zone);
                }
                else // tao moi
                {
                    zone = await zoneLogic.CreateAsync(new Zone
                    {
                        Id = Guid.NewGuid().ToString(),
                        ZoneName = ZoneNameBindProp,
                    });
                    MessagingCenter.Send(zone, Messages.ZONE_MESSAGE);
                }


                var param = new NavigationParameters();
                param.Add(Keys.ZONE, zone);
                    
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
            SaveCommand.ObservesProperty(() => ZoneNameBindProp);
        }

        #endregion

        #region Navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    if (parameters.ContainsKey(Keys.ZONE))
                    {
                        var zone = parameters[Keys.ZONE] as Zone;
                        ZoneNameBindProp = zone.ZoneName;
                        ZoneIdBindProp = zone.Id;
                        _isEditing = true;
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
        #endregion
    }
}
