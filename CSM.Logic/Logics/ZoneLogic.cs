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
    public class ZoneLogic : BaseLogic
    {
        public IReadOnlyList<string> ListAccessRoleCode = new List<string>
        {

        };

        public ZoneLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<Zone>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Zone> query = _DbContext.Zone;
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
        public Task<Zone> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Zone> query = _DbContext.Zone;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status);

            var item = query.FirstOrDefaultAsync(h => h.Id == id);

            return item;
        }
        public async Task<Zone> CreateAsync(Zone obj, bool saveChange = true)
        {
            var item = new Zone
            {
                Id = obj.Id,
                Creator = "Tam",
                ZoneName = obj.ZoneName,
                CreationDate = DateTime.Now.ToString(),
                IsDeleted = (int)IsDelete.Normal               
            };

            _DbContext.Zone.Add(item);

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return item;
        }
        public async Task<Zone> UpdateAsync(Zone obj, bool saveChange = true)
        {
            var item = await _DbContext.Zone.FirstOrDefaultAsync(h => h.Id == obj.Id);

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return item;
        }

        public async Task<bool> DeleteAsync(string id, bool saveChange = true)
        {
            var item = await _DbContext.Zone.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
            if (item == null)
            {
                return false;
            }

            // Remove cac bang lien quan

            // Remove bang chinh
            item.IsDeleted = (int)IsDelete.Deleted;

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return true;
        }
    }
}
