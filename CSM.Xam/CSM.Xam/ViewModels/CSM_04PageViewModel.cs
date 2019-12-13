using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSM.EFCore;
using CSM.Logic;
using CSM.Logic.Enums;
using CSM.Xam.Models;

namespace CSM.Xam.ViewModels
{
    public class CSM_04PageViewModel : ViewModelBase
    {
        private dataContext _dbContext = Helper.GetDataContext();
        public CSM_04PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            GetAllInvoice();
        }
        #region ListInvoiceBindProp
        private ObservableCollection<VisualInvoiceModel> _ListInvoiceBindProp = null;
        public ObservableCollection<VisualInvoiceModel> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        private async void GetAllInvoice()
        {
            var invoiceLogic = new InvoiceLogic(_dbContext);
            var invoiceItemLogic = new InvoiceItemLogic(_dbContext);
            var subItemLogic = new ItemItemOptionOrDiscountLogic(_dbContext);

            var listInvoice = await invoiceLogic.GetAllAsync(InvoiceStatus.Paid);
            var listVisualInvoice = Mapper.Map<List<VisualInvoiceModel>>(listInvoice);

            //foreach (var invoice in listVisualInvoice)
            //{
            //    var listInvoiceItem = await invoiceItemLogic.GetAsync(invoice.Id);
            //    foreach (var invoiceItem in listInvoiceItem)
            //    {
            //        if (invoiceItem.IsDiscount == 1)
            //        {
            //            var item = ListDiscountBindProp.First(h => h.Id == invoiceItem.FkItemOrDiscount);
            //            var visualItem = new VisualItemMenuModel
            //            {
            //                Id = item.Id,
            //                Quantity = invoiceItem.Quantity,
            //                Name = item.Name,
            //                Value = invoiceItem.Value,
            //            };
            //            invoice.ListDiscount.Add(visualItem);
            //        }
            //        else
            //        {
            //            var item = ListItem.First(h => h.Id == invoiceItem.FkItemOrDiscount);
            //            var listSubItem = await subItemLogic.GetAsync(item.Id);

            //            var visualItem = new VisualItemMenuModel
            //            {
            //                Id = item.Id,
            //                Quantity = invoiceItem.Quantity,
            //                Name = item.Name,
            //                Value = invoiceItem.Value,
            //            };
            //            visualItem.ListSubItem.Add(new VisualItemMenuModel
            //            {
            //                Name = "Đơn giá",
            //                Value = item.Value
            //            });

            //            foreach (var subItem in listSubItem)
            //            {
            //                var visualSubItem = ListDiscountBindProp.FirstOrDefault(h => h.Id == subItem.FkItemOptionOrDiscount);
            //                visualItem.ListSubItem.Add(new VisualItemMenuModel
            //                {
            //                    Id = visualSubItem.Id,
            //                    Name = visualSubItem.Name,
            //                    Value = subItem.Value,
            //                    Quantity = subItem.Quantity
            //                });
            //            }

            //            invoice.ListItemInBill.Add(visualItem);
            //            invoice.ItemCount += invoiceItem.Quantity;
            //        }
            //    }
            //}
            ListInvoiceBindProp = new ObservableCollection<VisualInvoiceModel>(listVisualInvoice);
        }
    }
}
