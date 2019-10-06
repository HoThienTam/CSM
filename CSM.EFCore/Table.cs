using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Table
    {
        public string Id { get; set; }
        public string TableName { get; set; }
        public long TableSize { get; set; }
        public long TableType { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkZone { get; set; }
        public string FkStore { get; set; }
    }
}
