using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.EFCore
{
    public partial class dataContext
    {
        public dataContext(string connectionString) : base(
            SqliteDbContextOptionsBuilderExtensions
                .UseSqlite(new DbContextOptionsBuilder(), connectionString)
                .Options
            )
        {
        }
    }
}
