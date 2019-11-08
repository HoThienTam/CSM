using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace CSM.Xam.ViewModels
{
    public class CSM_02_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_02_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

        }

        #region Bind Property

        #region ListCategoryBindProp
        private ObservableCollection<Category> _ListCategoryBindProp = null;
        public ObservableCollection<Category> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region IsEditingBindProp
        private bool _IsEditingBindProp = false;
        public bool IsEditingBindProp
        {
            get { return _IsEditingBindProp; }
            set { SetProperty(ref _IsEditingBindProp, value); }
        }
        #endregion

        #region NewCategoryNameBindProp
        private string _NewCategoryNameBindProp = string.Empty;
        public string NewCategoryNameBindProp
        {
            get { return _NewCategoryNameBindProp; }
            set { SetProperty(ref _NewCategoryNameBindProp, value); }
        }
        #endregion

        #endregion

        #region Command

        #region AddNewCategoryCommand

        public DelegateCommand<object> AddNewCategoryCommand { get; private set; }
        private bool CanExecuteAddNewCategory(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(NewCategoryNameBindProp))
            {
                return false;
            }
            return true;
        }
        private async void OnAddNewCategory(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var newCate = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryName = NewCategoryNameBindProp
                };
                ListCategoryBindProp.Add(newCate);
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
        private void InitAddNewCategoryCommand()
        {
            AddNewCategoryCommand = new DelegateCommand<object>(OnAddNewCategory);
            AddNewCategoryCommand.ObservesProperty(() => IsNotBusy);
            AddNewCategoryCommand.ObservesProperty(() => NewCategoryNameBindProp);
        }

        #endregion

        #region DeleteCategoryCommand

        public DelegateCommand<object> DeleteCategoryCommand { get; private set; }
        private async void OnDeleteCategory(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var resultConfirm = await PageDialogService.DisplayAlertAsync("Cảnh báo", "Bạn có chắc muốn xóa DANH MỤC này không?", "Có", "Không");
                if (resultConfirm)
                {
                    var cate = obj as Category;
                    ListCategoryBindProp.Remove(cate);
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
        private void InitDeleteCategoryCommand()
        {
            DeleteCategoryCommand = new DelegateCommand<object>(OnDeleteCategory);
            DeleteCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private async void OnSave(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (IsEditingBindProp) //neu dang o che do chinh sua
                {

                }
                else // neu dang o che do binh thuong
                {
                    // neu khong nhap gi thi go back
                    if (string.IsNullOrWhiteSpace(NewCategoryNameBindProp))
                    {                      
                        await NavigationService.GoBackAsync();
                    }
                    else // tao moi
                    {
                        var categoryLogic = new CategoryLogic(_dbContext);
                        var newCate = new Category
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryName = NewCategoryNameBindProp
                        };
                        await categoryLogic.CreateAsync(newCate);
                        ListCategoryBindProp.Add(newCate);

                        //truyen ten category ve trang tao item
                        var param = new NavigationParameters();
                        param.Add(Keys.CATEGORY, newCate.CategoryName);

                        await NavigationService.GoBackAsync(param);
                    }
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
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

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
                    var categoryLogic = new CategoryLogic(_dbContext);
                    var listCate = await categoryLogic.GetAllAsync();

                    ListCategoryBindProp = new ObservableCollection<Category>(listCate);
                    if (parameters.ContainsKey(Keys.IS_EDITING))
                    {
                        IsEditingBindProp = true;
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
