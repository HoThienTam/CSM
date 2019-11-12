using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CSM.Xam.ViewModels
{
    public class CSM_08PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_08PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

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
                // Neu tap vao khu vuc thi chinh sua
                if (obj is Zone zone)
                {
                    var param = new NavigationParameters();
                    param.Add(Keys.ZONE, zone);

                    await NavigationService.NavigateAsync(nameof(CSM_08_01Page), param);
                }
                else // tao moi
                {
                    await NavigationService.NavigateAsync(nameof(CSM_08_01Page));
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
        private void InitTapSectionCommand()
        {
            TapSectionCommand = new DelegateCommand<object>(OnTapSection);
            TapSectionCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region Navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey(Keys.ZONE))
                    {
                        var zone = parameters[Keys.ZONE] as Zone;
                        //Neu chua co thi add
                        if (!ListSectionBindProp.Any(i => i.Id == zone.Id))
                        {
                            ListSectionBindProp.Add(zone);
                        }
                        else // neu co roi thi update
                        {
                           ListSectionBindProp
                                .FirstOrDefault(i => i.Id == zone.Id)
                                .ZoneName = zone.ZoneName;
                        }
                    }
                    break;
                case NavigationMode.New:
                    var zoneLogic = new ZoneLogic(_dbContext);
                    var listZone = await zoneLogic.GetAllAsync();

                    ListSectionBindProp = new ObservableCollection<Zone>(listZone);
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
