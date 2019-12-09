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
    public class DiscountLogic : BaseLogic
    {
        public DiscountLogic(dataContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Discount>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Discount> query = _DbContext.Discount;
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
        public Task<Discount> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Discount> query = _DbContext.Discount;
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
        public async Task<Discount> CreateAsync(Discount obj, bool saveChange = true)
        {
            var item = new Discount
            {
                Id = obj.Id,
                Creator = "Tam",
                CreationDate = DateTime.Now.ToString(),
                DiscountName = obj.DiscountName,
                DiscountValue = obj.DiscountValue,
                FkStore = "1",
                IsDeleted = (int)IsDelete.Normal,
                IsInPercent = obj.IsInPercent,
                MaxValue = obj.MaxValue,
            };

            _DbContext.Discount.Add(item);

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
        public async Task<Discount> UpdateAsync(Discount obj, bool saveChange = true)
        {
            var item = await _DbContext.Discount.FirstOrDefaultAsync(h => h.Id == obj.Id);

            item.DiscountName = obj.DiscountName;
            item.DiscountValue = obj.DiscountValue;
            item.IsInPercent = obj.IsInPercent;
            item.MaxValue = obj.MaxValue;
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
            var item = await _DbContext.Discount.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
