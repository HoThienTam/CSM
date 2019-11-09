using CSM.EFCore;
using CSM.Logic;
using CSM.Logic.Enums;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;

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
        private ObservableCollection<CategoryExtended> _ListCategoryBindProp = null;
        public ObservableCollection<CategoryExtended> ListCategoryBindProp
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

        #region SelectedCategoryBindProp
        private CategoryExtended _SelectedCategoryBindProp = null;
        public CategoryExtended SelectedCategoryBindProp
        {
            get { return _SelectedCategoryBindProp; }
            set { SetProperty(ref _SelectedCategoryBindProp, value); }
        }
        #endregion

        #endregion

        #region Command

        #region SelectCategoryCommand

        public DelegateCommand<object> SelectCategoryCommand { get; private set; }
        private async void OnSelectCategory(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var selectedCategory = (obj as ItemTapCommandContext).Item as CategoryExtended;

                if (SelectedCategoryBindProp == selectedCategory)
                {
                    SelectedCategoryBindProp = null;
                }
                else
                {
                    SelectedCategoryBindProp = selectedCategory;
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
        private void InitSelectCategoryCommand()
        {
            SelectCategoryCommand = new DelegateCommand<object>(OnSelectCategory);
            SelectCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

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
                var newCate = new CategoryExtended
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryName = NewCategoryNameBindProp,
                    Status = Status.New
                };
                ListCategoryBindProp.Add(newCate);
                NewCategoryNameBindProp = string.Empty;
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
            AddNewCategoryCommand = new DelegateCommand<object>(OnAddNewCategory, CanExecuteAddNewCategory);
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
                    var cate = obj as CategoryExtended;

                    var categoryLogic = new CategoryLogic(_dbContext);
                    await categoryLogic.DeleteAsync(cate.Id, false);
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

        #region TextChangedCommand

        public DelegateCommand<object> TextChangedCommand { get; private set; }
        private async void OnTextChanged(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is CategoryExtended cate)
                {
                    cate.Status = Status.Modified;
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
        private void InitTextChangedCommand()
        {
            TextChangedCommand = new DelegateCommand<object>(OnTextChanged);
            TextChangedCommand.ObservesCanExecute(() => IsNotBusy);
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
                    var categoryLogic = new CategoryLogic(_dbContext);
                    // Kiem tra them xoa sua
                    foreach (var category in ListCategoryBindProp)
                    {
                        switch (category.Status)
                        {
                            case Status.New:
                                var newCategory = new CategoryExtended
                                {
                                    Id = category.Id,
                                    CategoryName = category.CategoryName
                                };
                                await categoryLogic.CreateAsync(newCategory, false);
                                break;
                            case Status.Normal:
                                break;
                            case Status.Modified:
                                var modCategory = new CategoryExtended
                                {
                                    Id = category.Id,
                                    CategoryName = category.CategoryName
                                };
                                await categoryLogic.UpdateAsync(modCategory, false);
                                break;
                            case Status.Deleted:
                                break;
                            default:
                                break;
                        }
                    }
                    //neu co nhap category moi thi tao roi quay ve
                    if (!string.IsNullOrWhiteSpace(NewCategoryNameBindProp))
                    {
                        var newCate = new CategoryExtended
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryName = NewCategoryNameBindProp
                        };
                        await categoryLogic.CreateAsync(newCate, false);
                        ListCategoryBindProp.Add(newCate);
                    }

                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    await NavigationService.GoBackAsync();
                }
                else // neu dang o che do binh thuong
                {
                    var categoryLogic = new CategoryLogic(_dbContext);
                    // Kiem tra them moi
                    foreach (var category in ListCategoryBindProp)
                    {
                        switch (category.Status)
                        {
                            case Status.New:
                                var newCategory = new CategoryExtended
                                {
                                    Id = category.Id,
                                    CategoryName = category.CategoryName,
                                };
                                await categoryLogic.CreateAsync(newCategory, false);
                                category.Status = Status.Normal;
                                break;
                            case Status.Normal:
                                break;
                            case Status.Modified:
                                break;
                            case Status.Deleted:
                                break;
                        }
                    }

                    // neu khong nhap gi
                    if (string.IsNullOrWhiteSpace(NewCategoryNameBindProp))
                    {
                        //neu co chon thi tra ve ten category
                        if (SelectedCategoryBindProp != null)
                        {
                            //truyen ten category ve trang tao item
                            var param = new NavigationParameters();
                            param.Add(Keys.CATEGORY, SelectedCategoryBindProp.CategoryName);

                            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                            await NavigationService.GoBackAsync(param);
                        }
                        else
                        {
                            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                            return;
                        }
                    }
                    else // tao moi
                    {
                        var newCate = new CategoryExtended
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryName = NewCategoryNameBindProp
                        };
                        await categoryLogic.CreateAsync(newCate, false);
                        ListCategoryBindProp.Add(newCate);

                        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

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
                    ListCategoryBindProp = new ObservableCollection<CategoryExtended>();

                    var categoryLogic = new CategoryLogic(_dbContext);
                    var listCate = await categoryLogic.GetAllAsync();

                    foreach (var category in listCate)
                    {
                        ListCategoryBindProp.Add(new CategoryExtended
                        {
                            Id = category.Id,
                            Status = Status.Normal,
                            CategoryName = category.CategoryName
                        });
                    }

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
