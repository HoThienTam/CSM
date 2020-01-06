using CSM.EFCore;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.SignalR.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendInvoice(Invoice invoice)
        {
            await Clients.Others.SendAsync("ReceiveInvoice", invoice);
        }
        public async Task SendInvoiceItem(InvoiceItemOrDiscount invoiceItem)
        {
            await Clients.Others.SendAsync("ReceiveInvoiceItem", invoiceItem);
        }
        public async Task SendItemDiscount(ItemDiscount itemDiscount)
        {
            await Clients.Others.SendAsync("ReceiveItemDiscount", itemDiscount);
        }
    }
}
