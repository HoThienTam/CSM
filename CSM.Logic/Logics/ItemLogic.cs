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
        public Task<List<Item>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
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

        public Task<Item> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Item> query = _DbContext.Item;
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
        public async Task<Item> CreateAsync(Item obj, bool saveChange = true)
        {
            var item = new Item
            {
                Id = obj.Id,
                CreationDate = DateTime.Now.ToString(),
                Creator = "Tam",
                FkStore = "fg",
                FkCategory = obj.FkCategory,
                IsStaticPrice = 1,
                FkWeightUnit = "a",
                IsChargedByWeight = 0,
                IsDeleted = (int)IsDelete.Normal,
                ItemImage = "a",
                ItemName = obj.ItemName,
                ManagementMethod = 0,
                Price = obj.Price
            };

            _DbContext.Item.Add(item);

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
        public async Task<Item> UpdateAsync(Item obj, bool saveChange = true)
        {
            var item = await _DbContext.Item.FirstOrDefaultAsync(h => h.Id == obj.Id);

            item.ItemName = obj.ItemName;
            item.FkCategory = obj.FkCategory;
            item.Price = obj.Price;
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
            var item = await _DbContext.Item.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
