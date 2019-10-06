using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Invoice
    {
        public string Id { get; set; }
        public double InvoiceNumber { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
        public long IsTakeAway { get; set; }
        public long Status { get; set; }
        public long CustomerCount { get; set; }
        public string FkTable { get; set; }
        public long PaymentMethod { get; set; }
        public string CloseDate { get; set; }
        public double TotalPrice { get; set; }
        public double PaidAmount { get; set; }
        public double RefundedAmount { get; set; }
        public double Tip { get; set; }
    }
}
