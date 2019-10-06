using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class ItemPriceMaterial
    {
        public string Id { get; set; }
        public string FkItemPrice { get; set; }
        public string FkMaterial { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDelete { get; set; }
        public double Quantity { get; set; }
    }
}
