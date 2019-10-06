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
        public long FkCategory { get; set; }
        public string FkWeightUnit { get; set; }
        public long ManagementMethod { get; set; }
        public long IsChargedByWeight { get; set; }
    }
}
