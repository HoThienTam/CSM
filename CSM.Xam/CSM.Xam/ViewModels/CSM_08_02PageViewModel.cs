using System;
using System.Collections.Generic;
using System.Text;
using CSM.EFCore;
using CSM.Logic.Enums;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;

namespace CSM.Xam.ViewModels
{
    public class CSM_08_02PageViewModel : ViewModelBase
    {
        public CSM_08_02PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Thêm bàn";
        }

        #region BindProp 

        #region ZoneIdBindProp
        private string _ZoneIdBindProp = string.Empty;
        public string ZoneIdBindProp
        {
            get { return _ZoneIdBindProp; }
            set { SetProperty(ref _ZoneIdBindProp, value); }
        }
        #endregion

        #region TableBindProp
        private VisualTableModel _TableBindProp = null;
        public VisualTableModel TableBindProp
        {
            get { return _TableBindProp; }
            set { SetProperty(ref _TableBindProp, value); }
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
            if (TableBindProp.TableSize == null && TableBindProp.TableType == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(TableBindProp.TableName))
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
                if (IsEditing)
                {
                    TableBindProp.Status = Status.Modified;
                    await NavigationService.GoBackAsync();
                }
                var param = new NavigationParameters();
                param.Add(Keys.TABLE, TableBindProp);
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
            SaveCommand.ObservesProperty(() => TableBindProp.TableName);
            SaveCommand.ObservesProperty(() => TableBindProp.TableSize);
            SaveCommand.ObservesProperty(() => TableBindProp.TableType);
        }

        #endregion

        #region SelectTypeCommand

        public DelegateCommand<object> SelectTypeCommand { get; private set; }
        private async void OnSelectType(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var type = obj as string;

                switch (type)
                {
                    case "Rect":
                        TableBindProp.TableType = (int)TableType.Rectangle;
                        break;
                    case "Square":
                        TableBindProp.TableType = (int)TableType.Square;
                        break;
                    case "Circle":
                        TableBindProp.TableType = (int)TableType.Circle;
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
        private void InitSelectTypeCommand()
        {
            SelectTypeCommand = new DelegateCommand<object>(OnSelectType);
            SelectTypeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectSizeCommand

        public DelegateCommand<object> SelectSizeCommand { get; private set; }
        private async void OnSelectSize(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var size = obj as string;

                switch (size)
                {
                    case "Small":
                        TableBindProp.TableSize = (int)TableSize.Small;
                        break;
                    case "Medium":
                        TableBindProp.TableSize = (int)TableSize.Medium;
                        break;
                    case "Large":
                        TableBindProp.TableSize = (int)TableSize.Large;
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
        private void InitSelectSizeCommand()
        {
            SelectSizeCommand = new DelegateCommand<object>(OnSelectSize);
            SelectSizeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteCommand

        public DelegateCommand<object> DeleteCommand { get; private set; }
        private async void OnDelete(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var accepted = await DisplayDeleteAlertAsync();
                if (accepted)
                {
                    TableBindProp.Status = Status.Deleted;
                    var param = new NavigationParameters();
                    param.Add(Keys.TABLE, TableBindProp);
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
        private void InitDeleteCommand()
        {
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            DeleteCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    ZoneIdBindProp = parameters[Keys.ZONE] as string;

                    if (parameters.ContainsKey(Keys.TABLE))
                    {
                        TableBindProp = parameters[Keys.TABLE] as VisualTableModel;
                        IsEditing = true;
                        Title = "Sửa bàn";
                    }
                    else
                    {
                        TableBindProp = new VisualTableModel();
                    }
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
            }
        }
    }
}
