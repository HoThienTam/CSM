using CSM.EFCore;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class ItemExtended : Item
    {
        public double Quantity { get; set; }
        public bool IsSelected { get; set; }
    }
}
