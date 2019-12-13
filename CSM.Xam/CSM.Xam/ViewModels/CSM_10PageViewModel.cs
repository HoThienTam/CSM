using CSM.Xam.Models;
using CSM.Xam.Views;
using CSM.Logic;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using CSM.EFCore;
using CSM.Logic.Enums;
using System.Threading.Tasks;

namespace CSM.Xam.ViewModels
{
    public class CSM_10PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_10PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {

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

        #region ReceivedMoneyBindProp
        private double _ReceivedMoneyBindProp = 0;
        public double ReceivedMoneyBindProp
        {
            get { return _ReceivedMoneyBindProp; }
            set { SetProperty(ref _ReceivedMoneyBindProp, value); }
        }
        #endregion

        #region TipBindProp
        private double _TipBindProp = 0;
        public double TipBindProp
        {
            get { return _TipBindProp; }
            set { SetProperty(ref _TipBindProp, value); }
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

        #region BillBindProp
        private VisualInvoiceModel _BillBindProp = null;
        public VisualInvoiceModel BillBindProp
        {
            get { return _BillBindProp; }
            set { SetProperty(ref _BillBindProp, value); }
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
                if (ReceivedMoneyBindProp > BillBindProp.TotalPrice)
                {
                    ChangeMoneyBindProp = ReceivedMoneyBindProp - BillBindProp.TotalPrice;
                }
                else
                {
                    ChangeMoneyBindProp = 0;
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
                    var invoiceLogic = new InvoiceLogic(_dbContext);
                    var invoiceItemLogic = new InvoiceItemLogic(_dbContext);
                    var tableLogic = new TableLogic(_dbContext);
                    var subItemLogic = new ItemItemOptionOrDiscountLogic(_dbContext);

                    var invoice = new Invoice
                    {
                        Id = BillBindProp.Id,
                        TotalPrice = BillBindProp.TotalPrice,
                        Status = (int)InvoiceStatus.Paid,
                        PaidAmount = ReceivedMoneyBindProp,
                        Tip = TipBindProp,
                        InvoiceNumber = await GenerateInvoiceNumber(),
                        IsTakeAway = BillBindProp.IsTakeAway,
                        FkTable = BillBindProp.FkTable,
                        CustomerCount = BillBindProp.CustomerCount
                    };

                    await tableLogic.ChangeStatusAsync(new Table
                    {
                        Id = BillBindProp.FkTable,
                        IsSelected = 0
                    }, false);

                    var inv = await invoiceLogic.CreateAsync(invoice, false);

                    foreach (var item in BillBindProp.ListItemInBill)
                    {
                        await invoiceItemLogic.CreateAsync(new InvoiceItemOrDiscount
                        {
                            FkInvoice = invoice.Id,
                            FkItemOrDiscount = item.Id,
                            Quantity = item.Quantity,
                            IsDiscount = 0,
                            Value = item.Value
                        }, false);

                        for (int i = 1; i < item.ListSubItem.Count; i++)
                        {
                            await subItemLogic.CreateAsync(new ItemItemOptionOrDiscount
                            {
                                FkItem = item.Id,
                                FkItemOptionOrDiscount = item.ListSubItem[i].Id,
                                IsDiscount = 1,
                                Quantity = item.ListSubItem[i].Quantity,
                                Value = item.ListSubItem[i].Value
                            }, false);
                        }
                    }

                    foreach (var item in BillBindProp.ListDiscount)
                    {
                        await invoiceItemLogic.CreateAsync(new InvoiceItemOrDiscount
                        {
                            FkInvoice = invoice.Id,
                            FkItemOrDiscount = item.Id,
                            Quantity = item.Quantity,
                            IsDiscount = 1,
                            Value = item.Value
                        }, false);
                    }

                    await _dbContext.SaveChangesAsync();
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

        #region TipCommand

        public DelegateCommand<object> TipCommand { get; private set; }
        private async void OnTip(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                TipBindProp = ChangeMoneyBindProp;
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
        private void InitTipCommand()
        {
            TipCommand = new DelegateCommand<object>(OnTip);
            TipCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion

        private async Task<string> GenerateInvoiceNumber()
        {
            var invoiceLogic = new InvoiceLogic(_dbContext);

            var listInvoice = await invoiceLogic.GetAllAsync(InvoiceStatus.Paid);
            return $"CP{listInvoice.Count}";
        }
        #region Navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    BillBindProp = parameters[Keys.BILL] as VisualInvoiceModel;
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
