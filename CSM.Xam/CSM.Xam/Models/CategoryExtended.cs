using CSM.EFCore;
using CSM.Logic.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualCategoryModel : BindableBase
    {
        public string Id { get; set; }

        #region CategoryName
        private string _CategoryName;
        public string CategoryName
        {
            get { return _CategoryName; }
            set { SetProperty(ref _CategoryName, value); }
        }
        #endregion

        public Status Status { get; set; }
    }
}
