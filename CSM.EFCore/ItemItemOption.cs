using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class ItemItemOption
    {
        public string FkItem { get; set; }
        public string FkItemOption { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
    }
}
