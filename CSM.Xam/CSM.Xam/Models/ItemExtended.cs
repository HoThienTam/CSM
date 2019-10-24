using CSM.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class ItemExtended : Item
    {
        public string PriceName { get; set; }
        public double Price { get; set; }
    }
}
