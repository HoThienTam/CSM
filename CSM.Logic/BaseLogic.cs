using CSM.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Logic
{
    public abstract class BaseLogic
    {
        protected dataContext _DbContext { get; private set; }

        public BaseLogic(dataContext dbContext)
        {
            _DbContext = dbContext;
        }

        protected void CheckRole(Employee employee, IReadOnlyList<string> listCheckRole)
        {

        }
    }
}
