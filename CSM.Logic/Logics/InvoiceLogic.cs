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
    public class InvoiceLogic : BaseLogic
    {
        public InvoiceLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<Invoice>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<Invoice> query = _DbContext.Invoice;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status).OrderBy(h => h.CreationDate);

            return query.ToListAsync();
        }

        public Task<Invoice> GetAsync(string id, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<Invoice> query = _DbContext.Invoice;
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

        public async Task<Invoice> CreateAsync(Invoice obj, bool saveChange = true)
        {
            var item = new Invoice
            {
                Id = obj.Id,
                CreationDate = DateTime.Now.ToString(),
                TotalPrice = obj.TotalPrice,
                Creator = "Tam",
                Status = obj.Status,
                FkStore = "fg",
                CloseDate = DateTime.Now.ToString(),
                CustomerCount = obj.CustomerCount,
                FkTable = obj.FkTable,
                PaidAmount = obj.PaidAmount,
                RefundedAmount = 0,
                Tip = obj.Tip,
                InvoiceNumber = obj.InvoiceNumber,
                IsDeleted = (int)IsDelete.Normal,
                IsTakeAway = obj.IsTakeAway,
                PaymentMethod = 0
            };

            _DbContext.Invoice.Add(item);

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

        public async Task<Invoice> UpdateAsync(Invoice obj, bool saveChange = true)
        {
            var item = await _DbContext.Invoice.FirstOrDefaultAsync(h => h.Id == obj.Id);

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
            var item = await _DbContext.Invoice.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
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
