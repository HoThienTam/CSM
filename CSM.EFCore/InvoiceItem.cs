using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class InvoiceItem
    {
        public string FkInvoice { get; set; }
        public string FkItem { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long Quantity { get; set; }
        public long IsDeleted { get; set; }
    }
}
