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
    public class EmployeeLogic : BaseLogic
    {
        public EmployeeLogic(dataContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Employee>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Employee> query = _DbContext.Employee;
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
        public Task<Employee> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Employee> query = _DbContext.Employee;
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

        public async Task<bool> LoginAsync(string username, string password)
        {

            var item = await _DbContext.Employee.FirstOrDefaultAsync(h => h.EmployeeName == username.Trim().ToLower() && h.Password == password.Trim().ToLower());

            if (item != null)
            {
                return true;
            }
            return false;
        }
        public async Task<Employee> CreateAsync(Employee obj, bool saveChange = true)
        {
            var item = new Employee
            {
                Id = obj.Id,
                Creator = "Tam",
                CreationDate = DateTime.Now.ToString(),
                IsDeleted = (int)IsDelete.Normal,
                Role = obj.Role,
                EmployeeName = obj.EmployeeName,
                FullName = obj.FullName,
                Password = obj.Password
            };

            _DbContext.Employee.Add(item);

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
        public async Task<Employee> UpdateAsync(Employee obj, bool saveChange = true)
        {
            var item = await _DbContext.Employee.FirstOrDefaultAsync(h => h.Id == obj.Id);

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
            var item = await _DbContext.Employee.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
