using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Logic.Enums;
using CSM.Xam.Models;
using Prism.Commands;
using Prism.Navigation;
using Telerik.XamarinForms.Input.Calendar;
using Item = CSM.EFCore.Item;

namespace CSM.Xam.ViewModels
{
    public class CSM_05PageViewModel : ViewModelBase
    {
        public CSM_05PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Doanh thu tổng quan";
            SideBarBindProp = new ObservableCollection<SideBarModel>
            {
                new SideBarModel {Title = "Doanh thu tổng quan", IsSelected = true },
                new SideBarModel {Title = "Mặt hàng bán chạy", IsSelected = false },
            };
        }

        #region bindprop

        #region OverallVisibleBindProp
        private bool _OverallVisibleBindProp = true;
        public bool OverallVisibleBindProp
        {
            get { return _OverallVisibleBindProp; }
            set { SetProperty(ref _OverallVisibleBindProp, value); }
        }
        #endregion

        #region TopSellersVisibleBindProp
        private bool _TopSellersVisibleBindProp = false;
        public bool TopSellersVisibleBindProp
        {
            get { return _TopSellersVisibleBindProp; }
            set { SetProperty(ref _TopSellersVisibleBindProp, value); }
        }
        #endregion

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

        #region RevenuePerDayVisibleBindProp
        private bool _RevenuePerDayVisibleBindProp = false;
        public bool RevenuePerDayVisibleBindProp
        {
            get { return _RevenuePerDayVisibleBindProp; }
            set { SetProperty(ref _RevenuePerDayVisibleBindProp, value); }
        }
        #endregion

        #region RevenuePerHourVisibleBindProp
        private bool _RevenuePerHourVisibleBindProp = false;
        public bool RevenuePerHourVisibleBindProp
        {
            get { return _RevenuePerHourVisibleBindProp; }
            set { SetProperty(ref _RevenuePerHourVisibleBindProp, value); }
        }
        #endregion

        #endregion

        #region TopSellersBindProp
        private ObservableCollection<RevenueModel> _TopSellersBindProp = null;
        public ObservableCollection<RevenueModel> TopSellersBindProp
        {
            get { return _TopSellersBindProp; }
            set { SetProperty(ref _TopSellersBindProp, value); }
        }
        #endregion

        #region ListInvoice
        private List<Invoice> _ListInvoice = null;
        public List<Invoice> ListInvoice
        {
            get { return _ListInvoice; }
            set { SetProperty(ref _ListInvoice, value); }
        }
        #endregion

        #region ListItem
        private List<Item> _ListItem = null;
        public List<Item> ListItem
        {
            get { return _ListItem; }
            set { SetProperty(ref _ListItem, value); }
        }
        #endregion

        #region ListInvoiceItem
        private List<InvoiceItemOrDiscount> _ListInvoiceItem = null;
        public List<InvoiceItemOrDiscount> ListInvoiceItem
        {
            get { return _ListInvoiceItem; }
            set { SetProperty(ref _ListInvoiceItem, value); }
        }
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

        #region Data
        private ObservableCollection<CategoricalData> _Data = null;
        public ObservableCollection<CategoricalData> Data
        {
            get { return _Data; }
            set { SetProperty(ref _Data, value); }
        }
        #endregion

        #region AverageData
        private ObservableCollection<DateTimeContinuousData> _AverageData = null;
        public ObservableCollection<DateTimeContinuousData> AverageData
        {
            get { return _AverageData; }
            set { SetProperty(ref _AverageData, value); }
        }
        #endregion

        #region Interval
        private double _Interval = 1;
        public double Interval
        {
            get { return _Interval; }
            set { SetProperty(ref _Interval, value); }
        }
        #endregion

        #region SideBarBindProp
        private ObservableCollection<SideBarModel> _SideBarBindProp = null;
        public ObservableCollection<SideBarModel> SideBarBindProp
        {
            get { return _SideBarBindProp; }
            set { SetProperty(ref _SideBarBindProp, value); }
        }
        #endregion

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
                    RevenuePerDayVisibleBindProp = false;
                }
                else
                {
                    SelectedDateBindProp = $"{DateRangeBindProp.From.ToShortDateString()} - {DateRangeBindProp.To.Date.ToShortDateString()}";
                    RevenuePerDayVisibleBindProp = true;
                }
                GetOverallData();
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

        #region TapSideBarCommand

        public DelegateCommand<object> TapSideBarCommand { get; private set; }
        private async void OnTapSideBar(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var sidebar = obj as SideBarModel;

                foreach (var bar in SideBarBindProp)
                {
                    bar.IsSelected = false;
                }
                switch (sidebar.Title)
                {
                    case "Doanh thu tổng quan":
                        OverallVisibleBindProp = true;
                        TopSellersVisibleBindProp = false;
                        GetOverallData();
                        break;
                    case "Mặt hàng bán chạy":
                        OverallVisibleBindProp = false;
                        TopSellersVisibleBindProp = true;
                        GetTopSeller();
                        break;
                }
                sidebar.IsSelected = true;
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
        private void InitTapSideBarCommand()
        {
            TapSideBarCommand = new DelegateCommand<object>(OnTapSideBar);
            TapSideBarCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        private async void GetOverallData()
        {
            try
            {

                TotalMoneyBindProp = 0;
                TotalTransactionBindProp = 0;
                //Doanh thu theo gio
                var data = new ObservableCollection<CategoricalData>();
                var value = new double[24];
                //Doanh thu theo ngay
                var averageData = new ObservableCollection<DateTimeContinuousData>();
                double days = 1;

                //Khoi tao list
                for (int i = 0; i < 24; i++)
                {
                    data.Add(new CategoricalData { Category = i, Value = 0 });
                    value[i] = 0;
                }
                days = (DateRangeBindProp.To - DateRangeBindProp.From).TotalDays + 1;
                for (int i = 0; i < days; i++)
                {
                    averageData.Add(new DateTimeContinuousData { DateTime = DateRangeBindProp.From.AddDays(i).ToString("dd/MM"), Value = 0 });
                }
                //Doanh thu tong quan
                foreach (var invoice in ListInvoice)
                {
                    if (Convert.ToDateTime(invoice.CloseDate).Date == DateRangeBindProp.From && DateRangeBindProp.From == DateRangeBindProp.To)
                    {
                        TotalMoneyBindProp += invoice.TotalPrice;
                        TotalTransactionBindProp++;
                        //Tinh tong value gio
                        var gio = Convert.ToDateTime(invoice.CloseDate).Hour;
                        value[gio] += invoice.TotalPrice;
                        //Tinh trung binh gio
                        data[gio].Value = value[gio] / days;
                    }
                    //Chon date range
                    else if (Convert.ToDateTime(invoice.CloseDate).Date >= DateRangeBindProp.From && Convert.ToDateTime(invoice.CloseDate).Date <= DateRangeBindProp.To)
                    {
                        TotalMoneyBindProp += invoice.TotalPrice;
                        TotalTransactionBindProp++;
                        var gio = Convert.ToDateTime(invoice.CloseDate).Hour;
                        var ngay = (Convert.ToDateTime(invoice.CloseDate).Date - DateRangeBindProp.From).Days;
                        value[gio] += invoice.TotalPrice;
                        data[gio].Value = value[gio] / days;
                        //Tinh value theo ngay
                        averageData[ngay].Value += invoice.TotalPrice;
                    }
                }

                //Nếu có giá trị mới hiện chart
                if (TotalMoneyBindProp > 0)
                {
                    AverageMoneyBindProp = TotalMoneyBindProp / TotalTransactionBindProp;
                    RevenuePerHourVisibleBindProp = true;
                }
                else
                {
                    AverageMoneyBindProp = 0;
                    RevenuePerHourVisibleBindProp = false;
                    RevenuePerDayVisibleBindProp = false;
                }
                // round to 8 ticks
                Interval = Math.Max(1, days / 8);
                Data = new ObservableCollection<CategoricalData>(data);
                AverageData = new ObservableCollection<DateTimeContinuousData>(averageData);
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
        }

        private void GetTopSeller()
        {
            TopSellersBindProp = new ObservableCollection<RevenueModel>();
            var invoiceItemLogic = new InvoiceItemLogic(Helper.GetDataContext());

            var listTopSeller = ListInvoice.Where(h => Convert.ToDateTime(h.CloseDate).Date >= DateRangeBindProp.From
                && Convert.ToDateTime(h.CloseDate).Date <= DateRangeBindProp.To)
                .Join(ListInvoiceItem,
                i => i.Id,
                e => e.FkInvoice,
                (i, e) => new
                {
                    e.Quantity,
                    e.Value,
                    e.FkItemOrDiscount
                }).Join(ListItem,
                i => i.FkItemOrDiscount,
                e => e.Id,
                (i, e) => new 
                { 
                    e.ItemName,
                    i.Quantity,
                    i.Value,
                    e.Id
                }).GroupBy(h => h.Id).Select(h => new
                {
                    Name = h.FirstOrDefault().ItemName,
                    Quantity = h.Sum(c => c.Quantity),
                    Revenue = h.Sum(c => c.Value),
                }).ToList().OrderByDescending(h => h.Quantity).Take(10);

            var totalTransaction = (int)listTopSeller.Sum(c => c.Quantity);
            var totalRevenue = listTopSeller.Sum(c => c.Revenue);
            // Tinh doanh thu theo danh muc
            foreach (var item in listTopSeller)
            {
                TopSellersBindProp.Add(new RevenueModel
                {
                    Name = item.Name,
                    Type = $"{Math.Round(item.Quantity / (double)totalTransaction * 100, 2)}%",
                    TransactionCount = (int)item.Quantity,
                    Revenue = item.Revenue
                });
            }
            //Tinh tong
            TopSellersBindProp.Add(new RevenueModel
            {
                Name = "Tổng",
                TransactionCount = totalTransaction,
                Revenue = totalRevenue
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    var invoiceLogic = new InvoiceLogic(Helper.GetDataContext());
                    var itemLogic = new ItemLogic(Helper.GetDataContext());
                    var invoiceItemLogic = new InvoiceItemLogic(Helper.GetDataContext());

                    DateRangeBindProp = new DateTimeRange(DateTime.Today.Date, DateTime.Today.Date);
                    ListInvoice = await invoiceLogic.GetAllAsync(InvoiceStatus.Paid);
                    ListItem = await itemLogic.GetAllAsync();
                    ListInvoiceItem = await invoiceItemLogic.GetAllAsync();
                    GetOverallData();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
            }
        }
    }
}

namespace CSM.Xam.Models
{
    public class CategoricalData
    {
        public int Category { get; set; }

        public double Value { get; set; }
    }

    public class DateTimeContinuousData
    {
        public string DateTime { get; set; }

        public double Value { get; set; }
    }

    public class RevenueModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int TransactionCount { get; set; }
        public double Revenue { get; set; }
    }
}

