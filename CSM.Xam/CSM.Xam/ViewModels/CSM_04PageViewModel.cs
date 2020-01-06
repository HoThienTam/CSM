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

namespace CSM.Xam.ViewModels
{
    public class CSM_04PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_04PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            GetAllInvoice();
            CurrentBillBindProp = ListInvoiceBindProp.First();
        }

        #region ListInvoiceBindProp
        private ObservableCollection<VisualInvoiceModel> _ListInvoiceBindProp = null;
        public ObservableCollection<VisualInvoiceModel> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        #region ListItem
        private List<VisualItemMenuModel> _ListItem = null;
        public List<VisualItemMenuModel> ListItem
        {
            get { return _ListItem; }
            set { SetProperty(ref _ListItem, value); }
        }
        #endregion

        #region ListDiscount
        private List<VisualItemMenuModel> _ListDiscount = null;
        public List<VisualItemMenuModel> ListDiscount
        {
            get { return _ListDiscount; }
            set { SetProperty(ref _ListDiscount, value); }
        }
        #endregion

        #region CurrentBillBindProp
        private VisualInvoiceModel _CurrentBillBindProp = null;
        public VisualInvoiceModel CurrentBillBindProp
        {
            get { return _CurrentBillBindProp; }
            set { SetProperty(ref _CurrentBillBindProp, value); }
        }
        #endregion

        #region SelectBillCommand

        public DelegateCommand<object> SelectBillCommand { get; private set; }
        private async void OnSelectBill(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var bill = obj as VisualInvoiceModel;
                CurrentBillBindProp = bill;
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
        private void InitSelectBillCommand()
        {
            SelectBillCommand = new DelegateCommand<object>(OnSelectBill);
            SelectBillCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        private async void GetAllInvoice()
        {
            try
            {
                var itemLogic = new ItemLogic(_dbContext);
                var discountLogic = new DiscountLogic(_dbContext);
                var invoiceLogic = new InvoiceLogic(_dbContext);
                var invoiceItemLogic = new InvoiceItemLogic(_dbContext);
                var subItemLogic = new ItemDiscountLogic(_dbContext);
                var tableLogic = new TableLogic(_dbContext);
                var zoneLogic = new ZoneLogic(_dbContext);

                var listTable = await tableLogic.GetAllAsync();
                var listInvoice = await invoiceLogic.GetAllAsync(InvoiceStatus.Paid);
                var listVisualInvoice = Mapper.Map<List<VisualInvoiceModel>>(listInvoice);
                var listZone = await zoneLogic.GetAllAsync();
                var listItem = await itemLogic.GetAllAsync();
                var listVisualItem = Mapper.Map<List<VisualItemMenuModel>>(listItem);
                var listDiscount = await discountLogic.GetAllAsync();
                var listVisualDiscount = Mapper.Map<List<VisualItemMenuModel>>(listDiscount);

                ListItem = new List<VisualItemMenuModel>(listVisualItem);
                ListDiscount = new List<VisualItemMenuModel>(listVisualDiscount);

                foreach (var invoice in listVisualInvoice)
                {
                    var listInvoiceItem = await invoiceItemLogic.GetAsync(invoice.Id);
                    foreach (var invoiceItem in listInvoiceItem)
                    {
                        if (invoiceItem.IsDiscount == 1)
                        {
                            var item = ListDiscount.First(h => h.Id == invoiceItem.FkItemOrDiscount);
                            var visualItem = new VisualItemMenuModel
                            {
                                Id = item.Id,
                                Quantity = invoiceItem.Quantity,
                                Name = item.Name,
                                Status = Status.Normal,
                                Value = invoiceItem.Value,
                            };
                            invoice.ListDiscount.Add(visualItem);
                        }
                        else
                        {
                            var item = ListItem.First(h => h.Id == invoiceItem.FkItemOrDiscount);
                            var listSubItem = await subItemLogic.GetAsync(item.Id);

                            var visualItem = new VisualItemMenuModel
                            {
                                Id = item.Id,
                                Quantity = invoiceItem.Quantity,
                                Name = item.Name,
                                Status = Status.Normal,
                                Value = invoiceItem.Value,
                            };
                            visualItem.ListSubItem.Add(new VisualItemMenuModel
                            {
                                Name = "Đơn giá",
                                Value = item.Value,
                            });

                            foreach (var subItem in listSubItem)
                            {
                                var visualSubItem = ListDiscount.FirstOrDefault(h => h.Id == subItem.FkDiscount);
                                visualItem.ListSubItem.Add(new VisualItemMenuModel
                                {
                                    Id = visualSubItem.Id,
                                    Name = visualSubItem.Name,
                                    Value = subItem.Value,
                                    Status = Status.Normal,
                                });
                            }

                            invoice.ListItemInBill.Add(visualItem);
                            invoice.ItemCount += invoiceItem.Quantity;
                            invoice.OriginalPrice += invoiceItem.Value;
                        }
                        if (invoice.IsTakeAway == 0)
                        {
                            var table = listTable.First(h => h.Id == invoice.FkTable);
                            var zone = listZone.First(h => h.Id == table.FkZone);
                            invoice.TableName = $"{zone.ZoneName} - {table.TableName}";
                        }
                        else
                        {
                            invoice.TableName = $"MANG ĐI";
                        }
                    }
                }
                ListInvoiceBindProp = new ObservableCollection<VisualInvoiceModel>(listVisualInvoice);

            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
        }
    }
}
