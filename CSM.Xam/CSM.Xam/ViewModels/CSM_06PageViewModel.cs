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
    public class CSM_06PageViewModel : ViewModelBase
    {
        public CSM_06PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            SideBarBindProp = new ObservableCollection<SideBarModel>
            {
                new SideBarModel{Title = "Danh sách hàng tồn", IsSelected = true},
                new SideBarModel{Title = "Hàng sắp hết", IsSelected = false},
                new SideBarModel{Title = "Lịch sử", IsSelected = false},
            };
            Title = "Danh sách hàng tồn";
        }

        #region RunOutItemVisibleBindProp   
        private bool _RunOutItemVisibleBindProp = false;
        public bool RunOutItemVisibleBindProp
        {
            get { return _RunOutItemVisibleBindProp; }
            set { SetProperty(ref _RunOutItemVisibleBindProp, value); }
        }
        #endregion

        #region ItemVisibleBindProp
        private bool _ItemVisibleBindProp = true;
        public bool ItemVisibleBindProp
        {
            get { return _ItemVisibleBindProp; }
            set { SetProperty(ref _ItemVisibleBindProp, value); }
        }
        #endregion

        #region HistoryVisibleBindProp
        private bool _HistoryVisibleBindProp = false;
        public bool HistoryVisibleBindProp
        {
            get { return _HistoryVisibleBindProp; }
            set { SetProperty(ref _HistoryVisibleBindProp, value); }
        }
        #endregion

        #region ListItemBindProp
        private ObservableCollection<VisualItemModel> _ListItemBindProp = null;
        public ObservableCollection<VisualItemModel> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
        }
        #endregion

        #region ListHistoryBindProp
        private ObservableCollection<VisualHistoryModel> _ListHistoryBindProp = null;
        public ObservableCollection<VisualHistoryModel> ListHistoryBindProp
        {
            get { return _ListHistoryBindProp; }
            set { SetProperty(ref _ListHistoryBindProp, value); }
        }
        #endregion

        #region ListRunOutItemBindProp
        private ObservableCollection<VisualItemModel> _ListRunOutItemBindProp = null;
        public ObservableCollection<VisualItemModel> ListRunOutItemBindProp
        {
            get { return _ListRunOutItemBindProp; }
            set { SetProperty(ref _ListRunOutItemBindProp, value); }
        }
        #endregion

        #region SideBarBindProp
        private ObservableCollection<SideBarModel> _SideBarBindProp = null;
        public ObservableCollection<SideBarModel> SideBarBindProp
        {
            get { return _SideBarBindProp; }
            set { SetProperty(ref _SideBarBindProp, value); }
        }
        #endregion

        #region SelectSideBarCommand

        public DelegateCommand<object> SelectSideBarCommand { get; private set; }
        private async void OnSelectSideBar(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                foreach (var sideBar in SideBarBindProp)
                {
                    sideBar.IsSelected = false;
                }
                var sidebar = obj as SideBarModel;
                sidebar.IsSelected = true;
                Title = sidebar.Title;
                switch (sidebar.Title)
                {
                    case "Danh sách hàng tồn":
                        RunOutItemVisibleBindProp = false;
                        HistoryVisibleBindProp = false;
                        ItemVisibleBindProp = true;
                        break;
                    case "Hàng sắp hết":
                        RunOutItemVisibleBindProp = true;
                        HistoryVisibleBindProp = false;
                        ItemVisibleBindProp = false;
                        break;
                    case "Lịch sử":
                        RunOutItemVisibleBindProp = false;
                        HistoryVisibleBindProp = true;
                        ItemVisibleBindProp = false;
                        break;
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
        private void InitSelectSideBarCommand()
        {
            SelectSideBarCommand = new DelegateCommand<object>(OnSelectSideBar);
            SelectSideBarCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region AddQuantityCommand

        public DelegateCommand<object> AddQuantityCommand { get; private set; }
        private async void OnAddQuantity(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is VisualItemModel item)
                {
                    var param = new NavigationParameters();
                    param.Add(Keys.ITEM, item);
                    await NavigationService.NavigateAsync(nameof(CSM_06_01Page), param);
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
        private void InitAddQuantityCommand()
        {
            AddQuantityCommand = new DelegateCommand<object>(OnAddQuantity);
            AddQuantityCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region MinusQuantityCommand

        public DelegateCommand<object> MinusQuantityCommand { get; private set; }
        private async void OnMinusQuantity(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is VisualItemModel item)
                {
                    var param = new NavigationParameters();
                    param.Add(Keys.ITEM, item);
                    await NavigationService.NavigateAsync(nameof(CSM_06_02Page), param);
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
        private void InitMinusQuantityCommand()
        {
            MinusQuantityCommand = new DelegateCommand<object>(OnMinusQuantity);
            MinusQuantityCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        private async void GetData()
        {
            var historyLogic = new ImportExportHistoryLogic(Helper.GetDataContext());
            var itemLogic = new ItemLogic(Helper.GetDataContext());

            var listHistory = await historyLogic.GetAllAsync();
            var listItem = await itemLogic.GetAllAsync();
            
            var listVisualItem = Mapper.Map<List<VisualItemModel>>(listItem.Where(h => h.IsManaged == 1 && h.CurrentQuantity > h.MinQuantity));
            var listVisualRunOutItem = Mapper.Map<List<VisualItemModel>>(listItem.Where(h => h.IsManaged == 1 && h.CurrentQuantity <= h.MinQuantity));
            var listVisualHistory = Mapper.Map<List<VisualHistoryModel>>(listHistory);

            foreach (var history in listVisualHistory)
            {
                history.Item = listItem.FirstOrDefault(h => h.Id == history.FkItemOrMaterial).ItemName;
            }

            ListItemBindProp = new ObservableCollection<VisualItemModel>(listVisualItem);
            ListRunOutItemBindProp = new ObservableCollection<VisualItemModel>(listVisualRunOutItem);
            ListHistoryBindProp = new ObservableCollection<VisualHistoryModel>(listVisualHistory);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey(Keys.HISTORY))
                    {
                        var history = parameters[Keys.HISTORY] as VisualHistoryModel;
                        ListHistoryBindProp.Insert(0, history);
                    }
                    break;
                case NavigationMode.New:
                    GetData();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
            }
        }
    }
}
