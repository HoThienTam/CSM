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
    public class ItemDiscountLogic : BaseLogic
    {
        public ItemDiscountLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<ItemDiscount>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<ItemDiscount> query = _DbContext.ItemDiscount;
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

        public Task<List<ItemDiscount>> GetAsync(string itemId, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<ItemDiscount> query = _DbContext.ItemDiscount;
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

        public async Task<ItemDiscount> CreateAsync(ItemDiscount obj, bool saveChange = true)
        {
            var item = new ItemDiscount
            {
                Id = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now.ToString(),
                Creator = "Tam",
                FkItem = obj.FkItem,
                FkDiscount = obj.FkDiscount,
                IsDeleted = (int)IsDelete.Normal,
                Value = obj.Value
            };

            _DbContext.ItemDiscount.Add(item);

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

        public async Task<ItemDiscount> UpdateAsync(ItemDiscount obj, bool saveChange = true)
        {
            var item = await _DbContext.ItemDiscount.FirstOrDefaultAsync(h => h.Id == obj.Id);

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
            var item = await _DbContext.ItemDiscount.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
