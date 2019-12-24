using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSM.Xam.Models;
using Prism.Commands;

namespace CSM.Xam.ViewModels
{
    public class CSM_06_02PageViewModel : ViewModelBase
    {
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

    }
}
