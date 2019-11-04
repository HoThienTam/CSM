using CSM.Xam.Models;
using CSM.Xam.Views;
using Prism.Commands;
using System;

namespace CSM.Xam.ViewModels
{
    public class CSM_10PageViewModel : ViewModelBase
    {
        public CSM_10PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Thanh toan";
        }
        #region Property

        #region ChangeMoneyBindProp
        private double _ChangeMoneyBindProp = 0;
        public double ChangeMoneyBindProp
        {
            get { return _ChangeMoneyBindProp; }
            set { SetProperty(ref _ChangeMoneyBindProp, value); }
        }
        #endregion

        #region TotalMoneyBindProp
        private double _TotalMoneyBindProp = 0;
        public double TotalMoneyBindProp
        {
            get { return _TotalMoneyBindProp; }
            set { SetProperty(ref _TotalMoneyBindProp, value); }
        }
        #endregion

        #region ReceivedMoneyBindProp
        private double _ReceivedMoneyBindProp = 0;
        public double ReceivedMoneyBindProp
        {
            get { return _ReceivedMoneyBindProp; }
            set { SetProperty(ref _ReceivedMoneyBindProp, value); }
        }
        #endregion

        #region IsCompletedBindProp
        private bool _IsCompletedBindProp = false;
        public bool IsCompletedBindProp
        {
            get { return _IsCompletedBindProp; }
            set { SetProperty(ref _IsCompletedBindProp, value); }
        }
        #endregion

        #endregion 

        #region Command

        #region TapNumberCommand

        public DelegateCommand<object> TapNumberCommand { get; private set; }
        private async void OnTapNumber(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day 
                if (obj is decimal d)
                {
                    if (ReceivedMoneyBindProp == 0)
                    {
                        ReceivedMoneyBindProp = (double)d;
                    }
                    else
                    {
                        ReceivedMoneyBindProp = ReceivedMoneyBindProp * 10 + (double)d;
                    }
                    return;
                }
                if (obj is string s)
                {
                    switch (s)
                    {
                        case "X":
                            if (ReceivedMoneyBindProp >= 10)
                            {
                                ReceivedMoneyBindProp = Math.Floor(ReceivedMoneyBindProp /= 10);
                            }
                            else
                            {
                                ReceivedMoneyBindProp = 0;
                            }
                            break;
                        case "C":
                            ReceivedMoneyBindProp = 0;
                            break;
                        case "00":
                            ReceivedMoneyBindProp *= 100;
                            break;
                        case "000":
                            ReceivedMoneyBindProp *= 1000;
                            break;
                    }
                }
                ChangeMoneyBindProp = ReceivedMoneyBindProp - TotalMoneyBindProp;
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
        private void InitTapNumberCommand()
        {
            TapNumberCommand = new DelegateCommand<object>(OnTapNumber);
            TapNumberCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region CompeletePaymentCommand
        public DelegateCommand<object> CompeletePaymentCommand { get; private set; }
        private async void OnCompeletePayment(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (!IsCompletedBindProp)
                {
                    IsCompletedBindProp = true;
                }
                else
                {
                    await NavigationService.NavigateAsync(nameof(MainPage));
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
        private void InitCompeletePaymentCommand()
        {
            CompeletePaymentCommand = new DelegateCommand<object>(OnCompeletePayment);
            CompeletePaymentCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion
    }
}
