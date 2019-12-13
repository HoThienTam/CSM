using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class ItemItemOptionOrDiscount
    {
        public string Id { get; set; }
        public string FkItem { get; set; }
        public string FkItemOptionOrDiscount { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public long IsDiscount { get; set; }
        public long Quantity { get; set; }
        public double Value { get; set; }
    }
}
