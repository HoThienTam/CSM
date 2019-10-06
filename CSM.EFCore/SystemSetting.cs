using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class SystemSetting
    {
        public string KeySetting { get; set; }
        public string SettingValue { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public string FkStore { get; set; }
    }
}
