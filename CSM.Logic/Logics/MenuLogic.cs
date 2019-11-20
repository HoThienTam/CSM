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
    public class MenuLogic : BaseLogic
    {
        public MenuLogic(dataContext dbContext) : base(dbContext)
        {

        }
        public Task<List<Menu>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Menu> query = _DbContext.Menu;
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
        public Task<Menu> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Menu> query = _DbContext.Menu;
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
        public async Task<Menu> CreateAsync(Menu obj, bool saveChange = true)
        {
            var item = new Menu
            {
                Id = obj.Id,
                Creator = "Tam",
                MenuName = obj.MenuName,
                ImageIcon = obj.ImageIcon,
                CreationDate = DateTime.Now.ToString(),
                FkStore = "1",
                IsDeleted = (int)IsDelete.Normal
            };

            _DbContext.Menu.Add(item);

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
        public async Task<Menu> UpdateAsync(Menu obj, bool saveChange = true)
        {
            var item = await _DbContext.Menu.FirstOrDefaultAsync(h => h.Id == obj.Id);

            item.MenuName = obj.MenuName;
            item.ImageIcon = obj.ImageIcon;

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
            var item = await _DbContext.Menu.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
