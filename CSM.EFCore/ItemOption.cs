using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class ItemOption
    {
        public string Id { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
        public string OptionName { get; set; }
        public double MinQuantity { get; set; }
        public double MaxQuantity { get; set; }
        public long CanSelectMultiple { get; set; }
    }
}
