using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;

namespace CSM.Xam.ViewModels
{
    public class CSM_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Chỉnh sửa trang";
            ListIcon = new ObservableCollection<string>();
            ListIcon.Add(FontAwesomeIcon.Salad);
            ListIcon.Add(FontAwesomeIcon.Beer);
            ListIcon.Add(FontAwesomeIcon.BreadSlice);
            ListIcon.Add(FontAwesomeIcon.Cocktail);
            ListIcon.Add(FontAwesomeIcon.Coffee);
            ListIcon.Add(FontAwesomeIcon.CoffeeTogo);
            ListIcon.Add(FontAwesomeIcon.FrenchFries);
            ListIcon.Add(FontAwesomeIcon.Pie);
            ListIcon.Add(FontAwesomeIcon.Soup);
            ListIcon.Add(FontAwesomeIcon.WineGlass);
            ListIcon.Add(FontAwesomeIcon.IceCream);
            ListIcon.Add(FontAwesomeIcon.Pizza);
            ListIcon.Add(FontAwesomeIcon.Burrito);
            ListIcon.Add(FontAwesomeIcon.Sandwich);
        }

        #region ListIcon
        private ObservableCollection<string> _ListIcon = null;
        public ObservableCollection<string> ListIcon
        {
            get { return _ListIcon; }
            set { SetProperty(ref _ListIcon, value); }
        }
        #endregion

        #region Menu
        private VisualMenuModel _Menu = null;
        public VisualMenuModel Menu
        {
            get { return _Menu; }
            set { SetProperty(ref _Menu, value); }
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
            if (string.IsNullOrWhiteSpace(Menu.MenuName))
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
                var menuLogic = new MenuLogic(_dbContext);

                await menuLogic.UpdateAsync(new Menu
                {
                    Id = Menu.Id,
                    ImageIcon = Menu.ImageIcon,
                    MenuName = Menu.MenuName
                });

                var param = new NavigationParameters();
                param.Add(Keys.MENU, Menu);
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
            SaveCommand.ObservesProperty(() => Menu.MenuName);
        }

        #endregion

        #region SelectIconCommand

        public DelegateCommand<object> SelectIconCommand { get; private set; }
        private async void OnSelectIcon(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var icon = obj as string;
                if (string.IsNullOrWhiteSpace(icon))
                {
                    Menu.ImageIcon = "\uf0f4";
                }
                else
                {
                    Menu.ImageIcon = icon;
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
        private void InitSelectIconCommand()
        {
            SelectIconCommand = new DelegateCommand<object>(OnSelectIcon);
            SelectIconCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    Menu = parameters[Keys.MENU] as VisualMenuModel;
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }
    }
}
