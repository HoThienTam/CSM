using CSM.EFCore;
using CSM.Logic.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Logic
{
    public class ItemLogic : BaseLogic
    {
        public IReadOnlyList<string> ListAccessRoleCode = new List<string>
        {

        };

        public ItemLogic(dataContext dbContext) : base(dbContext)
        {

        }
        public Task<List<Item>> GetAllAsync(Employee employee, IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            CheckRole(employee, ListAccessRoleCode);

            IQueryable<Item> query = _DbContext.Item;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status);

            return query.ToListAsync();
        }
    }
}
