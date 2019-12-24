using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSM.Xam.Models;
using Prism.Commands;

namespace CSM.Xam.ViewModels
{
    public class CSM_06_01PageViewModel : ViewModelBase
    {
        public CSM_06_01PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Thêm số lượng";
            ImportReasonsBindProp = new ObservableCollection<ReasonModel>
            {
                new ReasonModel{Reason = "Nhập thêm", IsSelected = false},
                new ReasonModel{Reason = "Điều chỉnh số dư", IsSelected = false}
            };
        }

        #region ImportReasonsBindProp
        private ObservableCollection<ReasonModel> _ImportReasonsBindProp = null;
        public ObservableCollection<ReasonModel> ImportReasonsBindProp
        {
            get { return _ImportReasonsBindProp; }
            set { SetProperty(ref _ImportReasonsBindProp, value); }
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
                foreach (var importReason in ImportReasonsBindProp)
                {
                    importReason.IsSelected = false;
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

    }
}
