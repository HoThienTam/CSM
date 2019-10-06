using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Material
    {
        public string Id { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public string MaterialName { get; set; }
        public string FkWeightUnit { get; set; }
        public double MinQuantity { get; set; }
        public double CurrentQuantity { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
    }
}
