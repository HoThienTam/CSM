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
    public class InvoiceItemLogic : BaseLogic
    {
        public InvoiceItemLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<InvoiceItem>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<InvoiceItem> query = _DbContext.InvoiceItem;
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

        public Task<InvoiceItem> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<InvoiceItem> query = _DbContext.InvoiceItem;
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

        public async Task<InvoiceItem> CreateAsync(InvoiceItem obj, bool saveChange = true)
        {
            var item = new InvoiceItem
            {
              Id = obj.Id,
              CreationDate = DateTime.Now.ToString("HH:mm:ss"),
              Creator = "Tam",
              FkInvoice = obj.FkInvoice,
              FkItem = obj.FkItem,
              IsDeleted = (int)IsDelete.Normal,
              Quantity = obj.Quantity
            };

            _DbContext.InvoiceItem.Add(item);

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

        public async Task<InvoiceItem> UpdateAsync(InvoiceItem obj, bool saveChange = true)
        {
            var item = await _DbContext.InvoiceItem.FirstOrDefaultAsync(h => h.Id == obj.Id);

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
            var item = await _DbContext.InvoiceItem.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
            if (item == null)
            {
                return false;
            }

            // Remove cac bang lien quan

            // Remove bang chinh

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
