using CSM.EFCore;
using CSM.Logic;
using CSM.Logic.Enums;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Xamarin.Forms;

namespace CSM.Xam.ViewModels
{
    public class CSM_02_01PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        private List<VisualCategoryModel> _listDeleted = new List<VisualCategoryModel>();
        public CSM_02_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

        }

        #region Bind Property

        #region ListCategoryBindProp
        private ObservableCollection<VisualCategoryModel> _ListCategoryBindProp = null;
        public ObservableCollection<VisualCategoryModel> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set 
            { 
                SetProperty(ref _IsEditing, value);
                RaisePropertyChanged(nameof(IsNotEditing));
            }
        }

        public bool IsNotEditing { get { return !_IsEditing; } }
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
        private VisualCategoryModel _SelectedCategoryBindProp = null;
        public VisualCategoryModel SelectedCategoryBindProp
        {
            get { return _SelectedCategoryBindProp; }
            set { SetProperty(ref _SelectedCategoryBindProp, value); }
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
                var newCate = new VisualCategoryModel
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
                var canDelete = await DisplayDeleteAlertAsync();
                if (canDelete)
                {
                    var cate = obj as VisualCategoryModel;

                    ListCategoryBindProp.Remove(cate);
                    _listDeleted.Add(cate);
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
                if (obj is VisualCategoryModel cate)
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
                if (IsEditing) //neu dang o che do chinh sua
                {
                    var categoryLogic = new CategoryLogic(_dbContext);
                    // Kiem tra them xoa sua
                    foreach (var category in ListCategoryBindProp)
                    {
                        switch (category.Status)
                        {
                            case Status.New:
                                var newCategory = new Category
                                {
                                    Id = category.Id,
                                    CategoryName = category.CategoryName
                                };
                                await categoryLogic.CreateAsync(newCategory, false);
                                break;
                            case Status.Normal:
                                break;
                            case Status.Modified:
                                var modCategory = new Category
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
                        MessagingCenter.Send(category, Messages.CATEGORY_MESSAGE);
                    }

                    foreach (var cate in _listDeleted)
                    {
                       cate.Status = Status.Deleted;
                       await categoryLogic.DeleteAsync(cate.Id, false);
                       MessagingCenter.Send(cate, Messages.CATEGORY_MESSAGE);

                    }
                    //neu co nhap category moi thi tao roi quay ve
                    if (!string.IsNullOrWhiteSpace(NewCategoryNameBindProp))
                    {
                        var newCate = new Category
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryName = NewCategoryNameBindProp,
                        };
                        var newVsCate = new VisualCategoryModel
                        {
                            Id = newCate.Id,
                            CategoryName = newCate.CategoryName,
                            Status = Status.New
                        };
                        await categoryLogic.CreateAsync(newCate, false);
                        ListCategoryBindProp.Add(newVsCate);
                        MessagingCenter.Send(newCate, Messages.CATEGORY_MESSAGE);
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
                                var newCategory = new Category
                                {
                                    Id = category.Id,
                                    CategoryName = category.CategoryName,
                                };
                                await categoryLogic.CreateAsync(newCategory, false);
                                MessagingCenter.Send(category, Messages.CATEGORY_MESSAGE);
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
                            param.Add(Keys.CATEGORY, SelectedCategoryBindProp);

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
                        var newCate = new Category
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryName = NewCategoryNameBindProp,
                        };
                        var newVsCate = new VisualCategoryModel
                        {
                            Id = newCate.Id,
                            CategoryName = newCate.CategoryName,
                            Status = Status.New
                        };
                        await categoryLogic.CreateAsync(newCate, false);
                        ListCategoryBindProp.Add(newVsCate);
                        MessagingCenter.Send(newCate, Messages.CATEGORY_MESSAGE);

                        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                        //truyen ten category ve trang tao item
                        var param = new NavigationParameters();
                        param.Add(Keys.CATEGORY, newCate);

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
                    ListCategoryBindProp = new ObservableCollection<VisualCategoryModel>();

                    var categoryLogic = new CategoryLogic(_dbContext);
                    var listCate = await categoryLogic.GetAllAsync();

                    foreach (var category in listCate)
                    {
                        ListCategoryBindProp.Add(new VisualCategoryModel
                        {
                            Id = category.Id,
                            Status = Status.Normal,
                            CategoryName = category.CategoryName
                        });
                    }

                    if (parameters.ContainsKey(Keys.IS_EDITING))
                    {
                        IsEditing = true;
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
