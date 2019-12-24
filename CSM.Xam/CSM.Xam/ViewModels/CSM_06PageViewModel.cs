using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            GetListItem();
        }

        #region ListItemBindProp
        private ObservableCollection<VisualItemModel> _ListItemBindProp = null;
        public ObservableCollection<VisualItemModel> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
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

        private async void GetListItem()
        {
            var itemLogic = new ItemLogic(Helper.GetDataContext());
            var listItem = await itemLogic.GetAllAsync();
            var listVisualItem = Mapper.Map<List<VisualItemModel>>(listItem.Where(h => h.IsManaged == 1));
            ListItemBindProp = new ObservableCollection<VisualItemModel>(listVisualItem);
        }
    }
}
