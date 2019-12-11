using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class InvoiceItemOrDiscount
    {
        public string FkInvoice { get; set; }
        public string FkItemOrDiscount { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long Quantity { get; set; }
        public long IsDeleted { get; set; }
        public long IsDiscount { get; set; }
    }
}
