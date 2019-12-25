using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualHistoryModel
    {
        public string Id { get; set; }
        public string CreationDate { get; set; }
        public string FkItemOrMaterial { get; set; }
        public long IsImported { get; set; }
        public string Reason { get; set; }
        public double Quantity { get; set; }
        public string Item { get; set; }
    }
}
