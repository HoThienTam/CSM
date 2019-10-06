using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class ImportExportHistory
    {
        public string Id { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FkStore { get; set; }
        public string FkItemOrMaterial { get; set; }
        public long IsImported { get; set; }
        public string Reason { get; set; }
        public double Quantity { get; set; }
    }
}
