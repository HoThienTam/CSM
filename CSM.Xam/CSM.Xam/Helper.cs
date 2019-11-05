using CSM.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam
{
    public class Helper
    {
        public static dataContext GetDataContext()
        {
            return new dataContext((App.Current as App).DbConnectionString);
        }
    }
}
