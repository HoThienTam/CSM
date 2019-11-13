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
    public class TableLogic : BaseLogic
    {
        public IReadOnlyList<string> ListAccessRoleCode = new List<string>
        {

        };

        public TableLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<Table>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Table> query = _DbContext.Table;
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

        public Task<List<Table>> GetTableByZoneAsync(string zoneId, IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Table> query = _DbContext.Table;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status);

            query = query.Where(h => h.FkZone == zoneId);

            return query.ToListAsync();
        }

        public Task<Table> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Table> query = _DbContext.Table;
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
        public async Task<Table> CreateAsync(Table obj, bool saveChange = true)
        {
            var item = new Table
            {
                Id = obj.Id,
                Creator = "Tam",
                CreationDate = DateTime.Now.ToString(),
                FkStore = "1",
                IsDeleted = (int)IsDelete.Normal,
                TableName = obj.TableName,
                TableSize = 1,
                FkZone = obj.FkZone,
                TableType = 1
            };

            _DbContext.Table.Add(item);

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
        public async Task<Table> UpdateAsync(Table obj, bool saveChange = true)
        {
            var item = await _DbContext.Table.FirstOrDefaultAsync(h => h.Id == obj.Id);

            item.TableName = obj.TableName;
            item.TableSize = obj.TableSize;
            item.TableType = obj.TableType;
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
            var item = await _DbContext.Table.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
