using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSM.EFCore;
using CSM.Logic.Enums;
using Microsoft.EntityFrameworkCore;

namespace CSM.Logic
{
    public class ImportExportHistoryLogic : BaseLogic
    {
        public ImportExportHistoryLogic(dataContext dbContext) : base(dbContext)
        {
        }
        public Task<List<ImportExportHistory>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<ImportExportHistory> query = _DbContext.ImportExportHistory;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status).OrderByDescending(h => h.CreationDate);

            return query.ToListAsync();
        }
        public Task<ImportExportHistory> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<ImportExportHistory> query = _DbContext.ImportExportHistory;
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
        public async Task<ImportExportHistory> CreateAsync(ImportExportHistory obj, bool saveChange = true)
        {
            var item = new ImportExportHistory
            {
                Id = Guid.NewGuid().ToString(),
                Creator = "Tam",
                CreationDate = DateTime.Now.ToString(),
                FkStore = "1",
                IsDeleted = (int)IsDelete.Normal,
                FkItemOrMaterial = obj.FkItemOrMaterial,
                IsImported = obj.IsImported,
                Quantity = obj.Quantity,
                Reason = obj.Reason
            };

            _DbContext.ImportExportHistory.Add(item);

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
        public async Task<ImportExportHistory> UpdateAsync(ImportExportHistory obj, bool saveChange = true)
        {
            var item = await _DbContext.ImportExportHistory.FirstOrDefaultAsync(h => h.Id == obj.Id);

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
            var item = await _DbContext.ImportExportHistory.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
