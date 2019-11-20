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
    class CSM_08_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_08_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Tạo khu vực";
        }

        #region Bind Prop

        #region ZoneBindProp
        private VisualZoneModel _ZoneBindProp = new VisualZoneModel();
        public VisualZoneModel ZoneBindProp
        {
            get { return _ZoneBindProp; }
            set { SetProperty(ref _ZoneBindProp, value); }
        }
        #endregion

        #region ListTableBindProp
        private ObservableCollection<Table> _ListTableBindProp = null;
        public ObservableCollection<Table> ListTableBindProp
        {
            get { return _ListTableBindProp; }
            set { SetProperty(ref _ListTableBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
        }
        #endregion

        #endregion

        #region Command

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(ZoneBindProp.ZoneName))
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
                var tableLogic = new TableLogic(_dbContext);
                var zoneLogic = new ZoneLogic(_dbContext);
                var zone = new Zone();
                //Neu chinh sua
                if (IsEditing)
                {
                    zone.Id = ZoneBindProp.Id;
                    zone.ZoneName = ZoneBindProp.ZoneName;

                    await zoneLogic.UpdateAsync(zone);

                    //gui thong tin den trang chu
                    MessagingCenter.Send(ZoneBindProp, Messages.ZONE_MESSAGE);

                    await NavigationService.GoBackAsync();
                }
                else // tao moi
                {
                    if (ZoneBindProp.Id == null)
                    {
                        ZoneBindProp.Id = Guid.NewGuid().ToString();
                    }
                    zone = await zoneLogic.CreateAsync(new Zone
                    {
                        Id = ZoneBindProp.Id,
                        ZoneName = ZoneBindProp.ZoneName,
                    }, false);
                    //gui ban da tao den trang chu
                    if (ListTableBindProp != null)
                    {
                        foreach (var table in ListTableBindProp)
                        {
                            await tableLogic.CreateAsync(table, false);
                        }
                        MessagingCenter.Send(ListTableBindProp, Messages.TABLE_MESSAGE);
                    }

                    //gui thong tin den trang chu
                    MessagingCenter.Send(ZoneBindProp, Messages.ZONE_MESSAGE);

                    var param = new NavigationParameters();
                    param.Add(Keys.ZONE, ZoneBindProp);

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
            SaveCommand.ObservesProperty(() => ZoneBindProp);
        }

        #endregion

        #region AddTableCommand

        public DelegateCommand<object> AddTableCommand { get; private set; }
        private async void OnAddTable(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (ListTableBindProp == null)
                {
                    ListTableBindProp = new ObservableCollection<Table>();
                }
                if (ZoneBindProp.Id == null)
                {
                    ZoneBindProp.Id = Guid.NewGuid().ToString();
                }
                var table = new Table
                {
                    Id = Guid.NewGuid().ToString(),
                    TableName = "001",
                    FkZone = ZoneBindProp.Id
                };
                ListTableBindProp.Add(table);
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
        private void InitAddTableCommand()
        {
            AddTableCommand = new DelegateCommand<object>(OnAddTable);
            AddTableCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteCommand

        public DelegateCommand<object> DeleteCommand { get; private set; }
        private bool CanExecuteDelete(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (!IsEditing)
            {
                return false;
            }
            return true;
        }
        private async void OnDelete(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var zoneLogic = new ZoneLogic(_dbContext);
                await zoneLogic.DeleteAsync(ZoneBindProp.Id);

                ZoneBindProp.IsDeleted = true;
                //gui thong tin den trang chu
                MessagingCenter.Send(ZoneBindProp, Messages.ZONE_MESSAGE);

                var param = new NavigationParameters();
                param.Add(Keys.ZONE, ZoneBindProp);

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
        private void InitDeleteCommand()
        {
            DeleteCommand = new DelegateCommand<object>(OnDelete, CanExecuteDelete);
            DeleteCommand.ObservesProperty(() => IsNotBusy);
        }

        #endregion

        #endregion

        #region Method

        private async void GetTable()
        {
            var tableLogic = new TableLogic(_dbContext);
            if (IsEditing)
            {
                var listTable = await tableLogic.GetTableByZoneAsync(ZoneBindProp.Id);
                ListTableBindProp = new ObservableCollection<Table>(listTable);
            }
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
                        var zone = parameters[Keys.ZONE] as VisualZoneModel;
                        ZoneBindProp = zone;
                        IsEditing = true;
                        Title = "Sửa khu vực";
                    }
                    GetTable();
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
