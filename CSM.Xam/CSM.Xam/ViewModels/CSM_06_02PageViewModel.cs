using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;

namespace CSM.Xam.ViewModels
{
    public class CSM_06_02PageViewModel : ViewModelBase
    {
        private string _reason;
        public CSM_06_02PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title= "Giảm số lượng";
            ExportReasonsBindProp = new ObservableCollection<ReasonModel>
            {
                new ReasonModel{Reason = "Bị hỏng", IsSelected = false},
                new ReasonModel{Reason = "Đã hết hạn", IsSelected = false},
                new ReasonModel{Reason = "Điều chỉnh số dư", IsSelected = false}
            };
        }

        #region ExportReasonsBindProp
        private ObservableCollection<ReasonModel> _ExportReasonsBindProp = null;
        public ObservableCollection<ReasonModel> ExportReasonsBindProp
        {
            get { return _ExportReasonsBindProp; }
            set { SetProperty(ref _ExportReasonsBindProp, value); }
        }
        #endregion

        #region QuantityBindProp
        private long _QuantityBindProp = 0;
        public long QuantityBindProp
        {
            get { return _QuantityBindProp; }
            set { SetProperty(ref _QuantityBindProp, value); }
        }
        #endregion

        #region OtherReasonBindProp
        private string _OtherReasonBindProp = string.Empty;
        public string OtherReasonBindProp
        {
            get { return _OtherReasonBindProp; }
            set { SetProperty(ref _OtherReasonBindProp, value); }
        }
        #endregion

        #region IsSelectingOtherReason
        private bool _IsSelectingOtherReason = false;
        public bool IsSelectingOtherReason
        {
            get { return _IsSelectingOtherReason; }
            set { SetProperty(ref _IsSelectingOtherReason, value); }
        }
        #endregion

        #region Item
        private VisualItemModel _Item = null;
        public VisualItemModel Item
        {
            get { return _Item; }
            set { SetProperty(ref _Item, value); }
        }
        #endregion

        #region SelectReasonCommand

        public DelegateCommand<object> SelectReasonCommand { get; private set; }
        private async void OnSelectReason(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                foreach (var exportReason in ExportReasonsBindProp)
                {
                    exportReason.IsSelected = false;
                }
                if (obj is ReasonModel)
                {
                    var reason = obj as ReasonModel;
                    reason.IsSelected = true;
                    IsSelectingOtherReason = false;
                    _reason = reason.Reason;
                }
                else
                {
                    IsSelectingOtherReason = true;
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
        private void InitSelectReasonCommand()
        {
            SelectReasonCommand = new DelegateCommand<object>(OnSelectReason);
            SelectReasonCommand.ObservesCanExecute(() => IsNotBusy);
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
                var dataContext = Helper.GetDataContext();
                var itemLogic = new ItemLogic(dataContext);
                var historyLogic = new ImportExportHistoryLogic(dataContext);

                var param = new NavigationParameters();
                //Update Số lượng Item và Material

                if (QuantityBindProp > 0)
                {
                    await itemLogic.ModifyQuantityAsync(Item.Id, -QuantityBindProp, false);

                    var history = new ImportExportHistory
                    {
                        FkItemOrMaterial = Item.Id,
                        IsImported = 0,
                        Quantity = -QuantityBindProp,
                        Reason = IsSelectingOtherReason == true ? OtherReasonBindProp : _reason
                    };
                    var his = await historyLogic.CreateAsync(history, false);

                    var visualHistory = new VisualHistoryModel
                    {
                        Quantity = history.Quantity,
                        Reason = history.Reason,
                        CreationDate = his.CreationDate,
                        Item = Item.ItemName
                    };

                    await dataContext.SaveChangesAsync();

                    Item.CurrentQuantity -= QuantityBindProp;

                    param.Add(Keys.HISTORY, visualHistory);
                    await NavigationService.GoBackAsync(param);
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Cảnh báo", "Bạn chưa nhập số lượng giảm!", "OK");
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    Item = parameters[Keys.ITEM] as VisualItemModel;
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
            }
        }
    }
}
