using System;
using System.Collections.Generic;

namespace CSM.EFCore
{
    public partial class Employee
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public long IsDeleted { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public long Role { get; set; }
    }
}
