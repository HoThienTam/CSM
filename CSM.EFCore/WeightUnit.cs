using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class WeightUnit
    {
        public string Id { get; set; }
        public string UnitName { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public string IsDeleted { get; set; }
        public string FkStore { get; set; }
    }
}
