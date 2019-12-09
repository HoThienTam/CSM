using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Discount
    {
        public string Id { get; set; }
        public string DiscountName { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
        public double DiscountValue { get; set; }
        public double MaxValue { get; set; }
        public long IsInPercent { get; set; }
    }
}
