using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class ItemPrice
    {
        public string Id { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string PriceName { get; set; }
        public double Price { get; set; }
        public string FkItem { get; set; }
        public long IsStaticPrice { get; set; }
    }
}
