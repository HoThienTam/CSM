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
    public class ItemItemOptionOrDiscountLogic : BaseLogic
    {
        public ItemItemOptionOrDiscountLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<ItemItemOptionOrDiscount>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<ItemItemOptionOrDiscount> query = _DbContext.ItemItemOptionOrDiscount;
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

        public Task<List<ItemItemOptionOrDiscount>> GetAsync(string itemId, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<ItemItemOptionOrDiscount> query = _DbContext.ItemItemOptionOrDiscount;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status);

            var item = query.Where(h => h.FkItem == itemId);

            return item.ToListAsync();
        }

        public async Task<ItemItemOptionOrDiscount> CreateAsync(ItemItemOptionOrDiscount obj, bool saveChange = true)
        {
            var item = new ItemItemOptionOrDiscount
            {
                Id = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now.ToString(),
                Creator = "Tam",
                FkItem = obj.FkItem,
                FkItemOptionOrDiscount = obj.FkItemOptionOrDiscount,
                IsDeleted = (int)IsDelete.Normal,
                IsDiscount = obj.IsDiscount,
                Quantity = obj.Quantity,
                Value = obj.Value
            };

            _DbContext.ItemItemOptionOrDiscount.Add(item);

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

        public async Task<ItemItemOptionOrDiscount> UpdateAsync(ItemItemOptionOrDiscount obj, bool saveChange = true)
        {
            var item = await _DbContext.ItemItemOptionOrDiscount.FirstOrDefaultAsync(h => h.Id == obj.Id);

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
            var item = await _DbContext.ItemItemOptionOrDiscount.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
