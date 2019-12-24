using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Item
    {
        public string Id { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public string FkCategory { get; set; }
        public long IsManaged { get; set; }
        public double Price { get; set; }
        public long MinQuantity { get; set; }
        public long CurrentQuantity { get; set; }
    }
}
