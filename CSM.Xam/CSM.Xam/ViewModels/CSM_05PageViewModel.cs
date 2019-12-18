using System;
using System.Collections.Generic;
using System.Text;
using CSM.Xam.Models;
using Prism.Commands;
using Telerik.XamarinForms.Input.Calendar;

namespace CSM.Xam.ViewModels
{
    public class CSM_05PageViewModel : ViewModelBase
    {
        public CSM_05PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Doanh thu tổng quan";
        }

        #region Doanh Thu Tong Quan

        #region TotalMoneyBindProp
        private double _TotalMoneyBindProp = 0;
        public double TotalMoneyBindProp
        {
            get { return _TotalMoneyBindProp; }
            set { SetProperty(ref _TotalMoneyBindProp, value); }
        }
        #endregion

        #region TotalTransactionBindProp
        private double _TotalTransactionBindProp = 0;
        public double TotalTransactionBindProp
        {
            get { return _TotalTransactionBindProp; }
            set { SetProperty(ref _TotalTransactionBindProp, value); }
        }
        #endregion

        #region AverageMoneyBindProp
        private double _AverageMoneyBindProp = 0;
        public double AverageMoneyBindProp
        {
            get { return _AverageMoneyBindProp; }
            set { SetProperty(ref _AverageMoneyBindProp, value); }
        }
        #endregion

        #region RevenuePerDayBindProp
        private bool _RevenuePerDayBindProp = false;
        public bool RevenuePerDayBindProp
        {
            get { return _RevenuePerDayBindProp; }
            set { SetProperty(ref _RevenuePerDayBindProp, value); }
        }
        #endregion

        #region RevenuePerHourBindProp
        private bool _RevenuePerHourBindProp = false;
        public bool RevenuePerHourBindProp
        {
            get { return _RevenuePerHourBindProp; }
            set { SetProperty(ref _RevenuePerHourBindProp, value); }
        }
        #endregion

        #endregion

        #region DateRangeBindProp
        private DateTimeRange _DateRangeBindProp = null;
        public DateTimeRange DateRangeBindProp
        {
            get { return _DateRangeBindProp; }
            set { SetProperty(ref _DateRangeBindProp, value); }
        }
        #endregion

        #region SelectedDateBindProp
        private string _SelectedDateBindProp = DateTime.Now.ToShortDateString();
        public string SelectedDateBindProp
        {
            get { return _SelectedDateBindProp; }
            set { SetProperty(ref _SelectedDateBindProp, value); }
        }
        #endregion

        #region PopupVisibleBindProp
        private bool _PopupVisibleBindProp = false;
        public bool PopupVisibleBindProp
        {
            get { return _PopupVisibleBindProp; }
            set { SetProperty(ref _PopupVisibleBindProp, value); }
        }
        #endregion

        #region OpenDatePickerCommand

        public DelegateCommand<object> OpenDatePickerCommand { get; private set; }
        private async void OnOpenDatePicker(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                PopupVisibleBindProp = true;
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
        private void InitOpenDatePickerCommand()
        {
            OpenDatePickerCommand = new DelegateCommand<object>(OnOpenDatePicker);
            OpenDatePickerCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region CloseDatePickerCommand

        public DelegateCommand<object> CloseDatePickerCommand { get; private set; }
        private async void OnCloseDatePicker(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                PopupVisibleBindProp = false;
                if (DateRangeBindProp.From == DateRangeBindProp.To)
                {
                    SelectedDateBindProp = DateRangeBindProp.From.ToShortDateString();
                    RevenuePerDayBindProp = false;
                }
                else
                {
                    SelectedDateBindProp = $"{DateRangeBindProp.From.ToShortDateString()} - {DateRangeBindProp.To.Date.ToShortDateString()}";
                    RevenuePerDayBindProp = true;
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
        private void InitCloseDatePickerCommand()
        {
            CloseDatePickerCommand = new DelegateCommand<object>(OnCloseDatePicker);
            CloseDatePickerCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region TapDateButtonCommand

        public DelegateCommand<object> TapDateButtonCommand { get; private set; }
        private async void OnTapDateButton(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                //lay ngay dau tien cua tuan nay
                var thisWeek = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(-(int)DateTime.Today.DayOfWeek + 1);
                //Lay ngay dau tien cua thang nay
                var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                if (obj is string _ID)
                {
                    switch (_ID)
                    {
                        case "today":
                            DateRangeBindProp = new DateTimeRange(DateTime.Today.Date, DateTime.Today.Date);
                            break;
                        case "yesterday":
                            DateRangeBindProp = new DateTimeRange(DateTime.Today.Date.AddDays(-1), DateTime.Today.Date.AddDays(-1));
                            break;
                        case "thisweek":
                            DateRangeBindProp = new DateTimeRange(thisWeek, DateTime.Now.Date);
                            break;
                        case "lastweek":
                            DateRangeBindProp = new DateTimeRange(thisWeek.AddDays(-7), thisWeek.AddDays(-1));
                            break;
                        case "thismonth":
                            DateRangeBindProp = new DateTimeRange(thisMonth, DateTime.Today.Date);
                            break;
                        case "lastmonth":
                            DateRangeBindProp = new DateTimeRange(thisMonth.AddMonths(-1), thisMonth.AddDays(-1));
                            break;
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
        private void InitTapDateButtonCommand()
        {
            TapDateButtonCommand = new DelegateCommand<object>(OnTapDateButton);
            TapDateButtonCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
