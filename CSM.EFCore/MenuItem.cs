using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class MenuItem
    {
        public string FkMenu { get; set; }
        public string FkItem { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
    }
}
