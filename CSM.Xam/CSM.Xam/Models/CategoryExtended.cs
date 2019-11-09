using CSM.EFCore;
using CSM.Logic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class CategoryExtended : Category
    {
        public Status Status { get; set; }
    }
}
