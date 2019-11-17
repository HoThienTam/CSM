using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSM.EFCore;
using Microsoft.EntityFrameworkCore;

namespace CSM.Logic
{
    public class MenuItemLogic : BaseLogic
    {
        public MenuItemLogic(dataContext dbContext) : base(dbContext)
        {

        }
        public Task<List<MenuItem>> GetAllAsync(bool tracking = false)
        {
            IQueryable<MenuItem> query = _DbContext.MenuItem;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            return query.ToListAsync();
        }
        public Task<List<MenuItem>> GetAsync(string menuId, bool tracking = false)
        {
            IQueryable<MenuItem> query = _DbContext.MenuItem;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            var item = query.Where(h => h.FkMenu == menuId);

            return item.ToListAsync();
        }
        public async Task<List<MenuItem>> CreateAsync(string menuId, List<Item> listItem, bool saveChange = true)
        {
            var listMenuItem = new List<MenuItem>();
            foreach (var item in listItem)
            {
                var menuItem = new MenuItem
                {
                    FkItem = item.Id,
                    FkMenu = menuId,
                    CreationDate = DateTime.UtcNow.ToString(),
                    Creator = "Tam"
                };

                listMenuItem.Add(menuItem);
                _DbContext.MenuItem.Add(menuItem);
            }

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Equals("SQLite Error 19: 'UNIQUE constraint failed: Menu_Item.fkMenu, Menu_Item.fkItem'."))
                {
                    //ignored   
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return listMenuItem;
        }
    }
}
