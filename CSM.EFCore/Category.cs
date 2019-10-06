using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Category
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
    }
}
